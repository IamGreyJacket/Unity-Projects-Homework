using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Arkanoid.Units.Player
{

    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private PlayerControls controls;
        [SerializeField, Tooltip("Скорость перемещения платформы.")]
        private float Speed = 1.5f;
        [SerializeField, Tooltip("Переменная, указывающая на то, управляется ли платформа Игроком 1. (Work in progress)")]
        private bool IsPlayer1 = false;
        private bool IsShot = false;
        UnityEngine.InputSystem.InputAction player;

        private void Awake()
        {
            controls = new PlayerControls();
        }
        private void OnEnable()
        {
            controls.GameMap.Enable();

            if (IsPlayer1)
            {
                player = controls.GameMap.MovementPlayer1;
                controls.GameMap.Shoot.started += OnShoot;
                controls.GameMap.ResetBall.started += OnResetBall;
            }
            else
            {
                player = controls.GameMap.MovementPlayer2;
            }
        }

        private IEnumerator OnMovement()
        {
            var translate = player.ReadValue<Vector2>();
            if (IsPlayer1)
            {
                _rigidbody.velocity += new Vector3(translate.x, translate.y, 0f) * Speed * Time.deltaTime;
                //transform.position += new Vector3(translate.x, translate.y, 0f) * Speed * Time.deltaTime;
                yield return null;
            }
            else
            {
                _rigidbody.velocity += new Vector3(translate.x * -1, translate.y, 0f) * Speed * Time.deltaTime;
                //transform.position += new Vector3(translate.x * -1, translate.y, 0f) * Speed * Time.deltaTime;
                yield return null;
            }
        }

        private void OnShoot(CallbackContext context)
        {
            IsShot = Managers.GameManager.Self.World.IsShot;
            if (!IsShot)
            {
                Debug.Log("The ball should be released");
                Managers.GameManager.Self.MoveBall();
            }
        }

        private void OnResetBall(CallbackContext context)
        {

            Managers.GameManager.Self.ResetBall();
        }

        private void OnDisable()
        {
            controls.GameMap.ResetBall.started -= OnResetBall;
            controls.GameMap.Shoot.started -= OnShoot;

            controls.GameMap.Disable();
        }
        private void OnDestroy()
        {
            controls.Dispose();
        }
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            StartCoroutine(OnMovement());
        }
    }
}
