using Cards.ScriptableObjects;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public delegate void Notifier();

    public class DeckManager : MonoBehaviour
    {
        //public event Notifier DrawCardEvent;

        private Material _baseMat;
        private List<CardPropertiesData> _allCards;
        private int _count = 0;

        private Card[] _playerDeck1;
        private Card[] _playerDeck2;

        [SerializeField]
        private int _maxCardInDeck = 30;
        [SerializeField]
        private Transform _deckPlayer1Parent;
        [SerializeField]
        private Transform _deckPlayer2Parent;

        [SerializeField, Space]
        private PlayerHand _playerHand1;
        [SerializeField]
        private PlayerHand _playerHand2;

        [SerializeField, Space]
        private Card _prefabCard;
        [SerializeField]
        private CardPackConfiguration[] _packs;

        private void Awake()
        {
            IEnumerable<CardPropertiesData> cards = new List<CardPropertiesData>();

            foreach (var pack in _packs) cards = pack.UnionProperties(cards);

            _allCards = new List<CardPropertiesData>(cards);

            _baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
            _baseMat.renderQueue = 2995;
        }

        private void Start()
        {
            _playerDeck1 = CreateDeck(_deckPlayer1Parent);
            _playerDeck2 = CreateDeck(_deckPlayer2Parent);
            GameManager.Self.ChangeTurnEvent += DrawCard;
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.E)) DrawCard(); //todo: изменить условие. Брать карту в начале хода либо при вызове способности карты.
            #endif
        }

        public void ChangeCard(Card handCard)
        {
            Card[] currentDeck;
            PlayerHand currentHand;
            if (GameManager.Self.IsPlayer1Turn)
            {
                currentDeck = _playerDeck1;
                currentHand = _playerHand1;
            }
            else
            {
                currentDeck = _playerDeck2;
                currentHand = _playerHand2;
            }
            var random = Random.Range(0, currentDeck.Length - 5); //Индекс случайной карты к колоде.
            var deckCard = currentDeck[random];
            int handPosIndex;
            for(int i = 0; i < currentHand.Cards.Length; i++)
            {
                if(currentHand.Cards[i] == handCard)
                {
                    handPosIndex = i;
                    var handPos = handCard.transform.position;

                    handCard.SwitchCard();
                    handCard.transform.position = deckCard.transform.position;
                    handCard.State = CardStateType.InDeck;
                    currentDeck[random] = handCard;

                    deckCard.transform.position = handPos;
                    currentHand.Cards[handPosIndex] = deckCard;
                    deckCard.SwitchCard();
                    deckCard.State = CardStateType.InHand;
                    deckCard.CanChange = false;
                    break;
                }
            }
        }

        public void SwitchCards()
        {
            _playerHand1.SwitchCards();
            _playerHand2.SwitchCards();
        }

        public void PlayCard(Card card)
        {
            switch (card.Player)
            {
                case PlayerType.Player1:
                    for(int i = 0; i < _playerHand1.Cards.Length; i++)
                    {
                        if (_playerHand1.Cards[i] == card)
                        {
                            _playerHand1.Cards[i] = null;
                            break;
                        }
                    }
                    break;
                case PlayerType.Player2:
                    for (int i = 0; i < _playerHand2.Cards.Length; i++)
                    {
                        if (_playerHand2.Cards[i] == card)
                        {
                            _playerHand2.Cards[i] = null;
                            break;
                        }
                    }
                    break;
            }
        }

        public void DrawCard()
        {
            if (GameManager.Self.IsPlayer1Turn)
            {
                for (int i = _playerDeck1.Length - 1; i >= 0; i--)
                {
                    if (_playerDeck1[i] != null)
                    {
                        _playerHand1.SetNewCard(_playerDeck1[i]);
                        _playerDeck1[i] = null;
                        return;
                    }
                }
                GameManager.Self.PunishHero(PlayerType.Player1, 1);
                return;
            }
            else
            {
                for (int i = _playerDeck2.Length - 1; i >= 0; i--)
                {
                    if (_playerDeck2[i] != null)
                    {
                        _playerHand2.SetNewCard(_playerDeck2[i]);
                        _playerDeck2[i] = null;
                        return;
                    }
                }
                GameManager.Self.PunishHero(PlayerType.Player2, 1);
                return;
            }
        }

        private Card[] CreateDeck(Transform parent)
        {
            var deck = new Card[_maxCardInDeck];
            var offset = new Vector3();
            for(int i = 0; i < _maxCardInDeck; i++)
            {
                deck[i] = Instantiate(_prefabCard, parent);
                deck[i].transform.localPosition = offset;
                offset += new Vector3(0f, 1f, 0f);

                var random = _allCards[Random.Range(0, _allCards.Count)];
                var newMat = new Material(_baseMat);
                newMat.mainTexture = random.Texture;
                deck[i].Configuration(random, newMat, CardUtility.GetDescriptionById(random.Id), _count);
            }
            _count++;
            return deck;
        }

        /*public void RotateCards()
        {
            for (int i = 0; i < _playerDeck1.Length; i++)
            {
                _playerHand1.RotateCards();
                _playerHand2.RotateCards();
            }
        }
        */
    }
}