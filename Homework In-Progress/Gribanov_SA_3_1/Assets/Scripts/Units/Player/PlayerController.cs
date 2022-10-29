using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Arkanoid.Units.Player
{

    public class PlayerController : MonoBehaviour
    {

        private PlayerControls controls;
        [SerializeField]
        private float Speed = 1.5f;
        [SerializeField]
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

        private void OnMovement()
        {
            var translate = player.ReadValue<Vector2>();
            if (IsPlayer1)
            {
                transform.position += new Vector3(0f, translate.y, translate.x * -1) * Speed * Time.deltaTime;
            }
            else
            {
                transform.position += new Vector3(0f, translate.y, translate.x) * Speed * Time.deltaTime;
            }
        }

        private void OnShoot(CallbackContext context)
        {
            if (!IsShot)
            {
                IsShot = true;
                //transform.Find("Playball");
                Debug.Log("The ball should be released");
                //Выпускает мячик в открытый полёт
                Managers.GameManager.Self.MoveMyBall();
            }
        }

        private void OnResetBall(CallbackContext context)
        {

            Managers.GameManager.Self.ResetBall();
            IsShot = false;
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

        }

        // Update is called once per frame
        void Update()
        {
            OnMovement();
        }
    }
}
