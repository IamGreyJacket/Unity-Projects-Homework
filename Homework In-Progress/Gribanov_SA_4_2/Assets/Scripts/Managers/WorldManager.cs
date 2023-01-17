using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Managers
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Список разрушаемых блоков на поле (Work in progress)")]
        private List<GameObject> _blocks;
        public int BlockCount { get; private set; } = 1;
        private static Vector3 s_rotator = new Vector3(1, 1, 1);
        [SerializeField, Tooltip("Переменная определяющая скорость вращения разрушаемых блоков.")]
        private float _rotationSpeed = 10f;

        [SerializeField, Tooltip("Ссылка на игровой мячик. (Work in progress)")]
        private GameObject _ball;
        [SerializeField, Tooltip("Скорость игрового мячика.")]
        private float _ballSpeed = 2;
        [Range(1, 1.5f)]
        private float _addSpeed = 1;
        [HideInInspector]
        public int PauseSpeed = 1;
        [HideInInspector]
        public bool IsShot = false;
        [HideInInspector]
        public Vector3 Direction;
        private Transform _startParent;

        private void OnValidate()
        {
            if(_blocks == null || _blocks.Count == 0)
            {
                throw new UnassignedReferenceException("There must be at least 1 block in WorldManager");
            }
            if (_ball == null)
            {
                throw new UnassignedReferenceException("Ball is not set in WorldManager");
            }
            if (_ball.name != "PlayBall" || !_ball.TryGetComponent<SphereCollider>(out SphereCollider component))
            {
                Debug.LogWarning("This GameObject either is not a PlayBall, or doesn't have SphereCollider\nBe careful, when using it as PlayBall");
            }
            if(_ballSpeed <= 0)
            {
                _ballSpeed = 2;
                Debug.LogWarning("Ball's speed cannot be 0 or less, it has to move forward");
            }
            if(_rotationSpeed == 0)
            {
                _rotationSpeed = 10f;
                Debug.LogWarning("Rotation speed cannot be 0, the blocks have to rotate");
            }
        }

        // Start is called before the first frame update
        void Start()
        {

            Direction = _ball.transform.forward;
            _startParent = _ball.transform.parent;
            StartCoroutine(RotateBlocks());
        }

        // Update is called once per frame
        void Update()
        {
            BlockCount = _blocks.Count;
        }

        #region World Management

        public void DestroyBlock(GameObject block)
        {
            _blocks.Remove(block);
            Destroy(block);
            Debug.Log("One of the blocks has been destroyed");
            Managers.LogManager.Self.Log("One of the blocks has been destroyed");
        }

        public void ResetBall()
        {
            //to do
            if (PauseSpeed == (float)PauseStatus.Paused) return;
            IsShot = false;
            Direction = _ball.transform.forward;
            _ball.transform.SetParent(_startParent);
            _ball.transform.position = _ball.transform.parent.transform.position + new Vector3(0,0,4);
            Debug.Log("The ball is reset");
            Managers.LogManager.Self.Log("The ball is reset");
        }

        public IEnumerator MoveBall()
        {
            if (PauseSpeed == (float)PauseStatus.Paused) yield return null;
            IsShot = true;
            _ball.transform.SetParent(null);
            Debug.Log("The ball started moving");
            Managers.LogManager.Self.Log("The ball started moving");
            while (IsShot)
            {
                _ball.transform.Translate(Direction * _addSpeed * _ballSpeed * PauseSpeed * Time.deltaTime);
                yield return null;
            }
            //Мячик должен двигаться вперед и изменять траекторию исходя из данных о столкновении
        }

        private IEnumerator RotateBlocks()
        {
            Debug.Log("Blocks started rotating");
            Managers.LogManager.Self.Log("Blocks started rotating");
            while (true)
            {

                foreach (var block in _blocks)
                {
                    block.transform.rotation *= Quaternion.Euler(s_rotator * _rotationSpeed * PauseSpeed * Time.deltaTime);
                }
                yield return null;
            }
        }

        public void PauseGame()
        {
            PauseSpeed = (int)PauseStatus.Paused;
            Time.timeScale = 0f;
            Debug.Log("The games is paused");
            Managers.LogManager.Self.Log("The game is paused");
        }

        public void UnpauseGame()
        {
            Time.timeScale = 1f;
            PauseSpeed = (int)PauseStatus.Playing;
            Debug.Log("The games is unpaused");
            Managers.LogManager.Self.Log("The game is unpaused");
        }

        public enum PauseStatus
        {
            Paused,
            Playing
        }
        #endregion
    }
}
