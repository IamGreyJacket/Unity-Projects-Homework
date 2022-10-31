using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Количество жизней на обоих игроков.")]
        private int health = 3;
        public static GameManager Self;
        [SerializeField, Tooltip("Ссылка на WorldManager (Work in Progress)")]
        public WorldManager World;
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
            if (health <= 0 || World.BlockCount <= 0) GameOver();
        }

        public void ReduceHealth()
        {
            health -= 1;
        }

        public void GameOver()
        {
            Debug.Log($"The game is over!");
            World.ResetBall(); //это я плохо сделал что сюда Ресет запихнул. Постоянно вызывается.
        }

        public void MoveBall()
        {
            StartCoroutine(World.MoveBall());
        }

        public void ResetBall()
        {
            World.ResetBall();
        }
    }
}
