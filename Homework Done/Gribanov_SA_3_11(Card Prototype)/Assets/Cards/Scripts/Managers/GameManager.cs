using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cards
{
    public class GameManager : MonoBehaviour
    {
        public event Notifier ChangeTurnEvent;

        public static GameManager Self;

        public bool IsStartPhase { get; private set; } = true;
        public bool IsFirstTurn { get; private set; } = true;

        [SerializeField]
        private GameObject _camera;

        private bool _isPlayer1Turn = true;

        [SerializeField]
        private GameObject _confirmWindow;

        [SerializeField]
        public TableManager TableManager;
        [SerializeField]
        public DeckManager DeckManager;
        [SerializeField]
        private PlayerHand[] _playerHands;
        [SerializeField]
        private PlayerHero[] _playerHeroes;


        public bool IsPlayer1Turn
        {
            get => _isPlayer1Turn;
            private set { _isPlayer1Turn = value; }
        }


        private void Awake()
        {
            Self = this;
        }

        private void Start()
        {
            _confirmWindow.SetActive(true);
            StartCoroutine(StartPhase());
        }

        private void Update()
        {
            
        }

        private void LateUpdate()
        {
            CheckHeroes();
        }

        private void CheckHeroes()
        {
            foreach(var hero in _playerHeroes)
            {
                if (hero.Health <= 0) GameOver(hero);
            }
        }

        public void PlayCard(Card card, Transform pos, out bool isPlayed)
        {
            TableManager.PlayCard(card, pos, out isPlayed);
            if (isPlayed) DeckManager.PlayCard(card);
        }

        /// <summary>
        /// Наказывает героя, нанося 1 единицу урона.
        /// </summary>
        public void PunishHero(PlayerType player, int damage)
        {
            foreach(var hero in _playerHeroes)
            {
                if (hero.Player == player)
                {
                    hero.Health -= damage;
                    Debug.Log($"{player}'s hero is damaged by {damage} points");
                    break;
                }
            }
        }

        private IEnumerator StartPhase()
        {
            for (int i = 0; i < 4; i++) { DeckManager.DrawCard(); }
            //Первая часть стартовой фазы. Первый игрок выбирает колоду.
            while (_confirmWindow.activeInHierarchy)
            { 
                yield return null;
            }
            ChangeTurn();
            for(int i = 0; i < 4; i++) { DeckManager.DrawCard(); }
            //вторая часть стартовой фазы. Второй игрок выбирает колоду.
            _confirmWindow.SetActive(true);
            while (_confirmWindow.activeInHierarchy)
            {
                yield return null;
            }
            ChangeTurn();
            IsStartPhase = false;
            //конец корутины без yield return;
        }

        private void GameOver(PlayerHero hero)
        {
            Debug.LogWarning($"Game Over! {hero.name} lost!");
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #endif
            //to do: научиться останавливать игру
            
        }

        public void ChangeCard(Card card)
        {
            if (IsStartPhase)
            {
                DeckManager.ChangeCard(card);
            }
        }

        public void ChangeTurn()
        {
            Debug.LogWarning("End of the turn!");
            if (IsPlayer1Turn)
            {
                IsPlayer1Turn = false;
            }
            else
            {
                IsPlayer1Turn = true;
            }
            DeckManager.SwitchCards();
            StartCoroutine(RotateCamera());
            RotateHeroes();
            RotateCards();
            //TableManager.AllCardsCanAttack();
            if (!IsStartPhase)
            {
                ChangeTurnEvent?.Invoke();
                IsFirstTurn = false;
            }
        }

        private void RotateCards()
        {
            TableManager.RotateCards();
        }

        private void RotateHeroes()
        {
            foreach(var hero in _playerHeroes)
            {
                hero.StartRotateHero();
            }
        }

        private IEnumerator RotateCamera()
        {
            var time = 0f;
            var startPos = _camera.transform.eulerAngles;
            var endPos = _camera.transform.eulerAngles + new Vector3(0f, 180f, 0f);
            while (time < 1f)
            {
                _camera.transform.eulerAngles = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime;
                yield return null;
            }
            _camera.transform.eulerAngles = endPos;
        }

    }
}