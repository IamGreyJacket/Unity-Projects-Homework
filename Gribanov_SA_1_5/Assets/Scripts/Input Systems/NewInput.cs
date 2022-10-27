using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class NewInput : MonoBehaviour
{
    public float JumpForce = 10;
    public MyInputActions controls;
    public GameObject _bullet;
    public Rigidbody _rigidbody;
    private List<GameObject> bullets;
    public float Speed = 5;
    private bool IsGrounded = true;
    private float bulletTime = 3;

    private void Awake()
    {
        controls = new MyInputActions();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IsGrounded = true;
        Debug.Log("Object is colliding with other object.");
    }

    private void OnEnable()
    {
        controls.PlayerOnFoot.Enable();

        controls.PlayerOnFoot.Jump.started += OnJump;
        controls.PlayerOnFoot.Fire.started += OnFire;
        controls.PlayerOnFoot.Log.started += OnLog;
    }

    public void OnMovement()
    {
        var translate = controls.PlayerOnFoot.Movement.ReadValue<Vector2>();
        float rotate = Convert.ToInt16(controls.PlayerOnFoot.Rotation.ReadValue<float>());
        transform.position += ((transform.forward * translate.y) + (transform.right * translate.x)) * Time.deltaTime * Speed;
        transform.eulerAngles += new Vector3(0f, rotate, 0);
    }

    public void OnJump(CallbackContext context)
    {
        if (IsGrounded)
        {
            _rigidbody.AddForce(JumpForce *Vector3.up, ForceMode.Impulse);
            IsGrounded = false;
        }

    }

    public void OnFire(CallbackContext context)
    {
        if (bulletTime > 2f)
        {
            Instantiate(_bullet, transform.position, transform.rotation);
            bulletTime = 0f;
        }
    }

    public void OnLog(CallbackContext context)
    {
        Debug.Log("Нажата либо клавиша F, либо Left Ctrl.");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnMovement();
        bulletTime += Time.deltaTime;
    }

    private void OnDisable()
    {
        controls.PlayerOnFoot.Jump.started -= OnJump;
        controls.PlayerOnFoot.Fire.started -= OnFire;
        controls.PlayerOnFoot.Log.started -= OnLog;

        controls.PlayerOnFoot.Disable();
    }

}
