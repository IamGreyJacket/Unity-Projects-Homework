using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Arkanoid.Units.Player
{

    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private PlayerControls _controls;
        [SerializeField, Tooltip("Скорость перемещения платформы.")]
        private float _speed = 1.5f;
        [SerializeField, Tooltip("Переменная, указывающая на то, управляется ли платформа Игроком 1. (Work in progress)")]
        private bool _isPlayer1 = false; //to do, не уверен как можно сделать так, чтобы только у одного объекта могла стоять эта галочка 

        private bool IsShot = false;
        UnityEngine.InputSystem.InputAction player;

        private void OnValidate()
        {
            if(_speed <= 0)
            {
                _speed = 1.5f;
                Debug.LogWarning("Player's speed cannot be 0 or less, he has to be able to move");
            }
        }

        private void Awake()
        {
            _controls = new PlayerControls();
        }
        private void OnEnable()
        {
            _controls.GameMap.Enable();
#if UNITY_EDITOR
            if (_isPlayer1)
            {
                player = _controls.GameMap.MovementPlayer1_Editor;
                _controls.GameMap.Pause.performed += OnPause;
                _controls.GameMap.Shoot.started += OnShoot;
                _controls.GameMap.ResetBall.started += OnResetBall;
                if (Debug.isDebugBuild) _controls.GameMap.Console_Dev.performed += OnConsole;
            }
            else
            {
                player = _controls.GameMap.MovementPlayer2_Editor;
            }
#else
            if (_isPlayer1)
            {
                player = _controls.GameMap.MovementPlayer1;
                _controls.GameMap.Pause.performed += OnPause;
                _controls.GameMap.Shoot.started += OnShoot;
                _controls.GameMap.ResetBall.started += OnResetBall;
                if (Debug.isDebugBuild) _controls.GameMap.Console_Dev.performed += OnConsole;
            }
            else
            {
                player = _controls.GameMap.MovementPlayer2;
            }
#endif
        }

        #region On Controls Input

        private IEnumerator OnMovement()
        {
            if (Managers.GameManager.Self.World.PauseSpeed == (int)Managers.WorldManager.PauseStatus.Paused) yield return null;
            else
            {
                var translate = player.ReadValue<Vector2>();
                if (_isPlayer1)
                {
                    _rigidbody.velocity += new Vector3(translate.x, translate.y, 0f) * _speed * Time.deltaTime;
                    //transform.position += new Vector3(translate.x, translate.y, 0f) * Speed * Time.deltaTime;
                    yield return null;
                }
                else
                {
                    _rigidbody.velocity += new Vector3(translate.x * -1, translate.y, 0f) * _speed * Time.deltaTime;
                    //transform.position += new Vector3(translate.x * -1, translate.y, 0f) * Speed * Time.deltaTime;
                    yield return null;
                }
            }
        }

        private void OnShoot(CallbackContext context)
        {
            if (Managers.GameManager.Self.World.PauseSpeed == (int)Managers.WorldManager.PauseStatus.Paused || Managers.GameManager.Self.IsGameOver) return;
            IsShot = Managers.GameManager.Self.World.IsShot;
            if (!IsShot && !Managers.GameManager.Self.IsGameOver)
            {
                Debug.Log("The ball is released");
                Managers.LogManager.Self.Log("The ball is released");
                Managers.GameManager.Self.MoveBall();
            }
        }

        private void OnResetBall(CallbackContext context)
        {
            Managers.GameManager.Self.ResetBall();
        }

        private void OnPause(CallbackContext context) 
        {
            Managers.GameManager.Self.PauseGame();
        }

        private void OnConsole(CallbackContext context)
        {
            if(Debug.developerConsoleVisible == false)
            {
                Debug.LogError("Force opening the console");
            }
            else
            {
                Debug.LogError("Closing the console");
                Debug.developerConsoleVisible = false;
            }
        }
        #endregion


        private void OnDisable()
        {
            _controls.GameMap.ResetBall.started -= OnResetBall;
            _controls.GameMap.Shoot.started -= OnShoot;
            _controls.GameMap.Pause.performed -= OnPause;
            if(Debug.isDebugBuild) _controls.GameMap.Console_Dev.performed -= OnConsole;

            _controls.GameMap.Disable();
        }
        private void OnDestroy()
        {
            _controls.Dispose();
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
