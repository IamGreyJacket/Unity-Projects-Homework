using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class NewPlayerController : MonoBehaviour
{
    [SerializeField]
    private float JumpForce = 10;
    public MyInputActions controls;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    protected float Speed = 5;
    [SerializeField]
    protected float SideSpeed = 5;
    private bool IsGrounded = true;

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
    }

    public void OnMovement()
    {
        var Move = controls.PlayerOnFoot.Movement.ReadValue<float>();
        var velocity = _rigidbody.velocity;
        velocity.z += Speed * Time.fixedDeltaTime;
        velocity.x += Move * SideSpeed * Time.fixedDeltaTime;
        _rigidbody.velocity = velocity;
        //transform.position += (transform.forward * Speed + Move * transform.right * SideSpeed) * Time.deltaTime;
    }

    public void OnJump(CallbackContext context)
    {
        if (IsGrounded)
        {
            _rigidbody.AddForce(JumpForce * Vector3.up, ForceMode.Impulse);
            IsGrounded = false;
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody.velocity = new Vector3(0, 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        OnMovement();
    }

    private void OnDisable()
    {
        controls.PlayerOnFoot.Jump.started -= OnJump;

        controls.PlayerOnFoot.Disable();
    }
}
