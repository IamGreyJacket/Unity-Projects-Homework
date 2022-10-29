using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int health = 3;
        public static GameManager Self;
        public WorldManager world;
        [SerializeField]
        private void Awake()
        {
            Self = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ReduceHealth()
        {
            health -= 1;
        }

        public void GameOver()
        {

        }

        public void MoveMyBall()
        {
            StartCoroutine(world.MoveBall());
        }

        public void ResetBall()
        {
            world.ResetBall();
        }
    }
}
