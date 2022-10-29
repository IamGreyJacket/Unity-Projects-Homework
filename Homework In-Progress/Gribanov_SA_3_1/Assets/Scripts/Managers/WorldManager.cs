using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Managers
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _blocks;
        private static Vector3 rotator = new Vector3(1, 1, 1);
        [SerializeField]
        private float rotationSpeed = 10f;
        [SerializeField]
        private GameObject _ball;
        [SerializeField]
        private float ballSpeed = 2;
        [Range(1, 1.5f)]
        private float addSpeed = 1;

        public void DestroyBlock(GameObject block)
        {
            _blocks.Remove(block);
            Destroy(block);
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(RotateBlocks());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ResetBall()
        {
            //to do
            
        }

        public IEnumerator MoveBall()
        {
            _ball.transform.SetParent(null);
            while (true)
            {
                _ball.transform.Translate(transform.forward * addSpeed * ballSpeed * Time.deltaTime);
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
