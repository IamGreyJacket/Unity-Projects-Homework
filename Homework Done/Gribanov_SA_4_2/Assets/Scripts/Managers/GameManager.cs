
using System.Collections;
using UnityEngine;

namespace Arkanoid.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Количество жизней на обоих игроков.")]
        private int _health = 3;

        public static GameManager Self { get; private set; }
        [SerializeField, Tooltip("Ссылка на WorldManager (Work in Progress)")]
        public WorldManager World;
        [SerializeField, Tooltip("Ссылка на контроллер паузы")]
        private Assistants.PauseController _pause;
        public bool IsGameOver { get; private set; } = false;

        private void OnValidate()
        {
            if(World == null)
            {
                throw new UnassignedReferenceException("WorldManager reference is unassigned in GameManager");
            }
            if(_pause == null)
            {
                throw new UnassignedReferenceException("PauseController reference is unassigned in GameManager");
            }
            if(_health <= 0)
            {
                _health = 3;
                Debug.LogWarning("Health cannot be 0 or less");
            }
        }

        private void Awake()
        {
            Self = this;
        }

        void Start()
        {
            StartCoroutine(GameCheck());
        }

        #region Game Management

        public void ReduceHealth()
        {
            _health -= 1;
            Managers.LogManager.Self.Log("Health is reduced by 1 point");
        }

        public void GameOver()
        {
            IsGameOver = true;
            Debug.Log("The game is over");
            Managers.LogManager.Self.Log("The game is over");
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

        public void PauseGame()
        {
            World.PauseGame();
            _pause.gameObject.SetActive(true);
        }
        public void UnpauseGame()
        {
            World.UnpauseGame();
        }

        private IEnumerator GameCheck()
        {
            while(_health > 0 && World.BlockCount > 0)
            { 
                Debug.LogWarning("Still checking the health");
                yield return null;
            }
            Debug.LogWarning("Now it's over!");
            GameOver();
            yield return null;
        }
        #endregion

        #region Info Getter
        public int GetHealth()
        {
            return _health;
        }
        #endregion
    }
}
