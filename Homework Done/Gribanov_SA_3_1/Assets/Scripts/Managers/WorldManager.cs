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
        private static Vector3 rotator = new Vector3(1, 1, 1);
        [SerializeField, Tooltip("Переменная определяющая скорость вращения разрушаемых блоков.")]
        private float rotationSpeed = 10f;
        [SerializeField, Tooltip("Ссылка на игровой мячик. (Work in progress)")]
        private GameObject _ball;
        [SerializeField, Tooltip("Скорость игрового мячика.")]
        private float ballSpeed = 2;
        [Range(1, 1.5f)]
        private float addSpeed = 1;
        public bool IsShot = false;
        public Vector3 _direction;
        private Vector3 startPoint;
        private Transform startParent;

        public void DestroyBlock(GameObject block)
        {
            _blocks.Remove(block);
            Destroy(block);
        }


        // Start is called before the first frame update
        void Start()
        {

            _direction = _ball.transform.forward;
            startParent = _ball.transform.parent;
            StartCoroutine(RotateBlocks());
        }

        // Update is called once per frame
        void Update()
        {
            BlockCount = _blocks.Count;
        }

        public void ResetBall()
        {
            //to do
            IsShot = false;
            Debug.Log($"Start Point: {startPoint}");
            _direction = _ball.transform.forward;
            _ball.transform.SetParent(startParent);
            _ball.transform.position = _ball.transform.parent.transform.position + new Vector3(0,0,4);
        }

        public IEnumerator MoveBall()
        {
            IsShot = true;
            _ball.transform.SetParent(null);
            while (IsShot)
            {
                _ball.transform.Translate(_direction * addSpeed * ballSpeed * Time.deltaTime);
                yield return null;
            }
            //Мячик должен двигаться вперед и изменять траекторию исходя из данных о столкновении
        }

        private IEnumerator RotateBlocks()
        {
            while (true)
            {

                foreach (var block in _blocks)
                {
                    block.transform.rotation *= Quaternion.Euler(rotator * rotationSpeed * Time.deltaTime);
                }
                yield return null;
            }
        }
    }
}
