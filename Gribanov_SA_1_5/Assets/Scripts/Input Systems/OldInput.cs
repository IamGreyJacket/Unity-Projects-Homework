using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldInput : MonoBehaviour
{
    public float JumpForce = 10;
    public Rigidbody _rigidbody;
    private bool IsGrounded = true;
    private List<GameObject> bullets;
    public float Speed;
    private float bulletTime = 3;
    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletTime += Time.deltaTime;
        StartCoroutine(Move());
        StartCoroutine(Logging());
        StartCoroutine(Shoot());
    }

    private void FixedUpdate()
    {
        StartCoroutine(Jump());
    }
    private void OnCollisionEnter(Collision collision)
    {
        IsGrounded = true;
    }

    private IEnumerator Move()
    {
        var x = Input.GetAxis("Horizontal Movement");
        var z = Input.GetAxis("Vertical Movement");
        var r = Input.GetAxis("Horizontal Rotation");
        transform.position += ((transform.forward * z) + (transform.right * x)) * Time.deltaTime * Speed;
        transform.eulerAngles += new Vector3(0f, r, 0);
        yield return null;
    }

    private IEnumerator Logging()
    {
        if (Input.GetAxis("Action Log") != 0)
        {
            Debug.Log("Нажата либо клавиша F, либо Left Ctrl.");
        }
        yield return null;
    }

    private IEnumerator Shoot()
    {
        if (Input.GetAxis("Fire1") != 0 & bulletTime > 2f)
        {
            GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = transform.position;
            bullet.transform.eulerAngles = transform.eulerAngles;
            bullet.AddComponent<ObjectMoving>();
            bullets.Add(bullet);
            bulletTime = 0f;
        }
        yield return null;
    }

    private IEnumerator Jump()
    {
        var j = Input.GetAxis("Jump");
        if (IsGrounded && j != 0)
        {
            _rigidbody.AddForce(JumpForce * Vector3.up, ForceMode.Impulse);
            IsGrounded = false;
        }
        yield return null;
    }
}
