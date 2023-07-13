using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        private static readonly Vector3 _stepPosition = new Vector3(0f, 5f, 0f);
        private const float _scale = 2f;
        [HideInInspector]
        public Vector3 StartScale { get; private set;}
        private GameManager _gameManager = GameManager.Self;

        public bool CanChange { get; set; } = true;

        private int _health;
        private int _maxHealth;
        private int _attack;
        private int _cost;
        private bool _switchedUp = false;
        private bool _rotated = false;
        private Color _healthColor;
        private CardUnitType _type;
        private PlayerType _playerType;
        private Vector3 _lastPos;
        private EffectData _effect;


        [SerializeField]
        private GameObject _frontCard;
        [SerializeField]
        private MeshRenderer _mesh;
        [SerializeField]
        private Animator _animator;

        [Space, SerializeField]
        private TextMeshPro _costT;
        [SerializeField]
        private TextMeshPro _nameT;
        [SerializeField]
        private TextMeshPro _descriptionT;
        [SerializeField]
        private TextMeshPro _attackT;
        [SerializeField]
        private TextMeshPro _healthT;
        [SerializeField]
        private TextMeshPro _typeT;

        public PlayerType Player
        {
            get => _playerType;
            set
            {
                _playerType = value;
            }
        }

        public Color HealthColor
        {
            get => _healthT.color;
            set
            {
                _healthColor = value;
                _healthT.color = _healthColor;
            }
        }

        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
            } 
        }

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                _healthT.text = _health.ToString();
            }
        }

        public int Attack
        {
            get => _attack;
            set
            {
                _attack = value;
                _attackT.text = _attack.ToString();
            }
        }
        public int Cost
        {
            get => _cost;
            private set
            {
                _cost = value;
                _costT.text = _cost.ToString();
            }
        }
        public CardUnitType Type
        {
            get => _type;
        }

        public EffectData Effect
        {
            get => _effect;
            private set 
            {
                _effect = value;
            }
        }

        public bool IsTaunt { get; set; } = false;
        public bool CanAttack { get; set; } = false;
        public CardStateType State { get; set; } = CardStateType.InDeck;

        private void Start()
        {
            StartScale = transform.localScale;
            
            //_lastPos = transform.position;
        }

        public void Configuration(CardPropertiesData data, Material mat, string description, int count)
        {
            _mesh.sharedMaterial = mat;
            _costT.text = (_cost = data.Cost).ToString();
            _nameT.text = data.Name;
            gameObject.name = data.Name;
            _descriptionT.text = description;
            _attackT.text = (_attack = data.Attack).ToString();
            _healthT.text = (_health = data.Health).ToString();
            MaxHealth = _health;
            _typeT.text = (_type = data.Type) == CardUnitType.None ? string.Empty : data.Type.ToString();
            _effect = data.Effect;
            if (_effect != null) 
            {
                Debug.Log($"{gameObject.name}'s effect is {_effect.GetType()}");
            }
            if (count == 1)
            {
                _playerType = PlayerType.Player2;
                Player = _playerType;
            }
            else
            {
                _playerType = PlayerType.Player1;
                Player = _playerType;
            }
        }

        public void UpdateCard()
        {
            _healthT.text = (_health = Health).ToString();
            _attackT.text = (_attack = Attack).ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (State)
            {
                case CardStateType.InHand:
                    transform.localPosition += _stepPosition;
                    transform.localScale *= _scale;
                    break;
                case CardStateType.OnTable:
                    transform.localPosition += _stepPosition;
                    transform.localScale *= _scale;
                    break;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            switch (State)
            {
                case CardStateType.InHand:
                    transform.localPosition -= _stepPosition;
                    transform.localScale = StartScale;
                    break;
                case CardStateType.OnTable:
                    transform.localPosition -= _stepPosition;
                    transform.localScale = StartScale;
                    break;
            }
        }

        [ContextMenu("Switch card")]
        public void SwitchCard()
        {
            transform.eulerAngles += new Vector3(0, 0, 180f);
            if (_switchedUp) 
            { 
                _animator.Play("Base Layer.BackSwitchCard");
                _switchedUp = false;
            }
            else
            {
                _animator.Play("Base Layer.SwitchCard");
                _switchedUp = true;
            }
        }


        public void StartRotateCard()
        {
            if (_rotated)
            {
                _animator.Play("Base Layer.BackRotateCard");
                _rotated = false;
            }
            else
            {
                _animator.Play("Base Layer.RotateCard");
                _rotated = true;
            }
            StartCoroutine(RotateCard());
        }

        private IEnumerator RotateCard()
        {
            var time = 0f;
            var startPos = transform.eulerAngles;
            var endPos = transform.eulerAngles + new Vector3(0f, 180f, 0f);
            while (time < 1f)
            {
                transform.eulerAngles = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime;
                yield return null;
            }
            transform.eulerAngles = endPos;
        }

        private Transform GetNearestPosition()
        {
            bool isSuccess;
            var res = _gameManager.TableManager.GetNearestPosition(this, out isSuccess); //_gameManager.IsPlayer1Turn,
            if (res != null)
            {
                Debug.Log(res.name);
            }
            if (isSuccess) return res;
            return res;
        }

        public void StartBattleAnimation()
        {
            _animator.Play("Base Layer.CardBattle");
        }

        private void PlayCard()
        {
            var nearest = GetNearestPosition();
            if (nearest != null)
            {
                _gameManager.PlayCard(this, nearest, out bool isPlayed);//todo поменять назначение isUpdated
                if (isPlayed)
                {
                    transform.position = nearest.transform.position + new Vector3(0f, 5f, 0f);
                    _lastPos = nearest.transform.position;
                    State = CardStateType.OnTable;
                }
                else
                {
                    transform.position = _lastPos;
                }
            }
            else
            {
                transform.position = _lastPos;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_gameManager.IsStartPhase)
            {
                if (CanChange) _gameManager.ChangeCard(this);
                transform.localScale = StartScale;
            }
            else
            {
                _lastPos = transform.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Должны узнать какая позиция или карта ближе всего к нам.
            //Сразу после делаем либо нанесение урона карте либо примагничиваемся к позиции.
            if (!_gameManager.IsStartPhase)
            {
                if (!_gameManager.TableManager.IsChoosingTarget)
                {
                    Transform nearest;
                    //поиск ближайшей позиции или врага
                    switch (State)
                    {
                        case CardStateType.InHand:
                            if ((_playerType == PlayerType.Player1 && _gameManager.IsPlayer1Turn) || (_playerType == PlayerType.Player2 && !_gameManager.IsPlayer1Turn))
                            {
                                PlayCard();
                            }
                            break;
                        case CardStateType.OnTable:
                            nearest = GetNearestPosition();
                            if (nearest != null && CanAttack)
                            {
                                _gameManager.TableManager.BattleCards(this, nearest, out bool isSuccess);
                                if (isSuccess) CanAttack = false;
                            }
                            if (CanAttack)
                            {
                                if (_gameManager.TableManager.TryBattleHero(this)) { Debug.LogWarning("Hero is HIT!"); CanAttack = false; };
                            }
                            //to do активация анимации
                            transform.position = _lastPos;
                            break;
                    }
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_gameManager.IsStartPhase)
            {
                if (!_gameManager.TableManager.IsChoosingTarget)
                {
                    switch (State)
                    {
                        case CardStateType.InHand:
                            if ((_playerType == PlayerType.Player1 && _gameManager.IsPlayer1Turn) || (_playerType == PlayerType.Player2 && !_gameManager.IsPlayer1Turn))
                            {
                                transform.position = eventData.pointerCurrentRaycast.worldPosition;
                                break;
                            }
                            break;

                        case (CardStateType.OnTable):
                            if ((_playerType == PlayerType.Player1 && _gameManager.IsPlayer1Turn) || (_playerType == PlayerType.Player2 && !_gameManager.IsPlayer1Turn))
                            {
                                transform.position = eventData.pointerCurrentRaycast.worldPosition;
                                break;
                            }
                            break;
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //to do
            if (_gameManager.IsStartPhase)
            {
                if (CanChange) _gameManager.ChangeCard(this);
                transform.localScale = StartScale;
            }
            switch (State)
            {
                case CardStateType.OnTable:
                    if (_gameManager.TableManager.IsChoosingTarget) _gameManager.TableManager.SetTarget(this);
                    break;
            }
        }
    }
}
