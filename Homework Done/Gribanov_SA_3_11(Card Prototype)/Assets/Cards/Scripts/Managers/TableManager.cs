using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Cards
{
    public delegate void EffectNotifier(Card card);
    public class TableManager : MonoBehaviour
    {
        public event EffectNotifier DamageTakenEvent;
        //public event Notifier SetTargetCardEvent;

        [SerializeField]
        private GameObject _effectHintWindow;

        [SerializeField]
        private Transform[] _table1Positions;
        [SerializeField]
        private Transform[] _table2Positions;

        [SerializeField]
        private PlayerHero _player1Hero;
        [SerializeField]
        private PlayerHero _player2Hero;

        private Dictionary<Transform, Card> _table1Cards;
        private Dictionary<Transform, Card> _table2Cards;
        private Dictionary<Card, List<Card>> _tableAffectedCards = new Dictionary<Card, List<Card>>(); //Ключ - карта наносящий эффект, значение - карта с нанесенным эффектом
        private Dictionary<Card, BuffEffect> _tableEffects = new Dictionary<Card, BuffEffect>(); //Ключ - карта с эффектом баффа, значение - сам эффект баффа.

        [HideInInspector]
        public byte TauntCardsCountPlayer1 = 0;
        [HideInInspector]
        public byte TauntCardsCountPlayer2 = 0;
        [HideInInspector]
        public bool IsChoosingTarget = false;
        private Object _target = null;

        private void Start()
        {
            _table1Cards = new Dictionary<Transform, Card>(_table1Positions.Length);
            _table2Cards = new Dictionary<Transform, Card>(_table2Positions.Length);
            for(int i = 0; i < _table1Positions.Length; i++)
            {
                _table1Cards[_table1Positions[i]] = null;
                _table2Cards[_table2Positions[i]] = null;
            }
            GameManager.Self.ChangeTurnEvent += AllCardsCanAttack;
        }

        public Transform GetNearestPosition(Card card, out bool isSuccess) //уточнить когда может вызываться.
        {
            Transform[] currentTablePositions;
            List<float> distances = new List<float>();
            if (card.Player == PlayerType.Player1)
            {
                if (card.State == CardStateType.InHand) currentTablePositions = _table1Positions;
                else currentTablePositions = _table2Positions;
            }
            else
            {
                if (card.State == CardStateType.InHand) currentTablePositions = _table2Positions;
                else currentTablePositions = _table1Positions;
            }

            foreach (var pos in currentTablePositions)
            {
                distances.Add(Vector3.Distance(card.transform.position, pos.position));
            }
            List<float> temp = new List<float>(distances);
            temp.Sort();
            if (temp[0] > 150f) { isSuccess = false; return null; }
            var res = currentTablePositions[distances.IndexOf(temp[0])]; //to do Определение враг это или просто позиция.
            isSuccess = true;
            //if(card.State == CardStateType.InHand) UpdatePositionCard(card, res, out isSuccess);
            if (!isSuccess) return null;
            return res;
        }

        public void PlayCard(Card card, Transform pos, out bool isPlayed)
        {
            Dictionary<Transform, Card> currentTableCards;
            PlayerHero currentPlayerHero;
            //Метод, обновляющий данные о позиции. Например если позиция пустая, то в неё записывается карта.
            //Если маны героя не хватает, то метод ничего не делает и в out bool isPlayed возвращает false;
            //Если маны героя достаточно, то мана отнимается, а карта привязывается к позиции.
            if(card.Player == PlayerType.Player1)
            {
                currentPlayerHero = _player1Hero;
                currentTableCards = _table1Cards;
            }
            else
            {
                currentPlayerHero = _player2Hero;
                currentTableCards = _table2Cards;
            }

            if (currentTableCards[pos] == null && currentPlayerHero.Mana >= card.Cost)
            {
                currentPlayerHero.Mana -= card.Cost;
                currentTableCards[pos] = card;
                if (card.Effect != null) card.Effect.Apply(card);
                ApplyExistingEffects(card);
                isPlayed = true;
            }
            else
            {
                isPlayed = false;
            }
        }

        public int CountCards(PlayerType player, CardUnitType type)
        {
            Dictionary<Transform, Card> currentTableCards;
            int count = 0;
            if (player == PlayerType.Player1) currentTableCards = _table1Cards;
            else currentTableCards = _table2Cards;
            if(type == CardUnitType.None)
            {
                foreach(var card in currentTableCards)
                {
                    if (card.Value != null) count++;
                }
            }
            else
            {
                foreach (var card in currentTableCards)
                {
                    if (card.Value != null && card.Value.Type == type) count++;
                }
            }
            return count;
        }

        public void ApplyExistingEffects(Card targetCard)
        {
            if (_tableEffects != null)
            {
                foreach (var effect in _tableEffects)
                {
                    if (targetCard.Player == effect.Key.Player)
                    {
                        if (effect.Value.Type == CardUnitType.None)
                        {
                            targetCard.Attack += effect.Value.Attack;
                            targetCard.MaxHealth += effect.Value.Health;
                            targetCard.Health += effect.Value.Health;
                            _tableAffectedCards[effect.Key].Add(targetCard);
                        }
                        else if (targetCard.Type == effect.Value.Type)
                        {
                            targetCard.Attack += effect.Value.Attack;
                            targetCard.MaxHealth += effect.Value.Health;
                            targetCard.Health += effect.Value.Health;
                            _tableAffectedCards[effect.Key].Add(targetCard);
                        }
                        
                    }
                }
            }
        }

        public void SummonCard(CardPropertiesData cardData, Card card, PlayerType player)
        {
            Dictionary<Transform, Card> currentTableCards;
            Transform[] currentTablePositions;
            if (player == PlayerType.Player1)
            {
                currentTableCards = _table1Cards;
                currentTablePositions = _table1Positions;
            }
            else
            {
                currentTableCards = _table2Cards;
                currentTablePositions = _table2Positions;
            }
            byte count;
            if (player == PlayerType.Player1) count = 0;
            else
            {
                count = 1;
            }
            var baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
            baseMat.renderQueue = 2995;
            var newMat = new Material(baseMat);
            newMat.mainTexture = cardData.Texture;
            card.Configuration(cardData, newMat, CardUtility.GetDescriptionById(cardData.Id), count);
            card.Player = player;

            count = 0;
            bool isFound = false;
            foreach(var pos in currentTableCards.Values)
            {
                if (pos == null)
                {
                    isFound = true;
                    break;
                }
                count++;
            }
            if (isFound)
            {
                currentTableCards[currentTablePositions[count]] = card;
                card.State = CardStateType.OnTable;
                card.transform.position = currentTablePositions[count].transform.position;
                if (card.Player == PlayerType.Player2) 
                { 
                    card.transform.eulerAngles += new Vector3(0, 180, 0);
                    card.StartRotateCard();
                }
                card.SwitchCard();
                ApplyExistingEffects(card);
            }
            else
            {
                Destroy(card.gameObject);
            }
        }

        public void RotateCards()
        {
            foreach(var card in _table1Cards.Values)
            {
                if (card != null) card.StartRotateCard();
            }
            foreach (var card in _table2Cards.Values)
            {
                if (card != null) card.StartRotateCard();
            }
        }

        public void AllCardsCanAttack()
        {
            foreach(var card in _table1Cards.Values)
            {
                if (card == null) continue;
                card.CanAttack = true;
            }
            foreach (var card in _table2Cards.Values)
            {
                if (card == null) continue;
                card.CanAttack = true;
            }

        }

        public bool TryBattleHero(Card card)
        {
            if (card.Player == PlayerType.Player1)
            {
                var distance = Vector3.Distance(card.transform.position, _player2Hero.transform.position);
                if (distance > 150f) return false;
                _player2Hero.Health -= card.Attack;
                return true;
            }
            else
            {
                var distance = Vector3.Distance(card.transform.position, _player1Hero.transform.position);
                if (distance > 150f) return false;
                _player1Hero.Health -= card.Attack;
                return true;
            }
        }

        private IEnumerator MoveBattleCards(Card card, Card enemyCard)
        {
            card.StartBattleAnimation();
            enemyCard.StartBattleAnimation();
            yield return new WaitForSeconds(1f);
            enemyCard.StartRotateCard();
            enemyCard.StartRotateCard();
            CheckCardHealth(card);
            CheckCardHealth(enemyCard);
        }

        public void BattleCards(Card card, Transform nearest, out bool isSuccess) //
        {
            Dictionary<Transform, Card> currentTableCards;
            byte currentTauntCount;
            if (card.Player == PlayerType.Player1)
            {
                currentTableCards = _table2Cards;
                currentTauntCount = TauntCardsCountPlayer2;
                
            }
            else 
            { 
                currentTableCards = _table1Cards;
                currentTauntCount = TauntCardsCountPlayer1;
            }

            var enemyCard = currentTableCards[nearest];
            if (enemyCard == null) isSuccess = false;
            else
            {
                if ((currentTauntCount > 0 & enemyCard.IsTaunt) || currentTauntCount <= 0)
                {
                    Debug.Log("Card are fighting");
                    StartCoroutine(MoveBattleCards(card, enemyCard));
                    enemyCard.Health -= card.Attack;
                    card.Health -= enemyCard.Attack;
                    card.HealthColor = Color.red;
                    enemyCard.HealthColor = Color.red;
                    card.CanAttack = false;
                    DamageTakenEvent?.Invoke(card);
                    DamageTakenEvent?.Invoke(enemyCard);
                    //to do проверка эффектов, расы, таунтов и прочего
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
        }

        public void CheckCardHealth(Card card)
        {
            if (card.Health <= 0)
            {
                Debug.Log($"{card.Player}'s card {card.name} is DEAD");
                if (card.Effect != null) 
                {
                    if (card.Effect.GetType() == typeof(BuffEffect) && ((BuffEffect)card.Effect).Target == EffectTargetType.AllCard)
                    {
                        if(_tableAffectedCards != null && _tableAffectedCards.ContainsKey(card) && _tableAffectedCards[card] != null)
                        foreach(var _card in _tableAffectedCards[card])
                        {
                            if (_card != null) _tableEffects[card].Discard(_card);
                        }
                        _tableAffectedCards[card].Clear();
                        _tableAffectedCards.Remove(card);
                        _tableEffects[card] = null;
                        _tableEffects.Remove(card);
                    }
                    else
                    {
                        card.Effect.Discard(card);
                    }
                }
                Destroy(card.gameObject);
            }
        }

        public void BuffCards(Card card, CardUnitType type, int attack, int health, bool isPermanent)
        {
            Dictionary<Transform, Card> currentTableCards;
            if (card.Player == PlayerType.Player1) 
            {
                currentTableCards = _table1Cards; 
            }
            else 
            { 
                currentTableCards = _table2Cards; 
            }

            List<Card> affectedCards = new List<Card>();
            if (type == CardUnitType.None)
            {
                foreach (var tabCard in currentTableCards)
                {
                    if (tabCard.Value != null && tabCard.Value != card)
                    {
                        tabCard.Value.Attack += attack;
                        tabCard.Value.MaxHealth += health;
                        tabCard.Value.Health += health;
                        if (!isPermanent) affectedCards.Add(tabCard.Value);
                    }
                }
            }
            else
            {
                foreach (var tabCard in currentTableCards)
                {
                    if(tabCard.Value != null && tabCard.Value != card && tabCard.Value.Type == type)
                    {
                        tabCard.Value.Attack += attack;
                        tabCard.Value.MaxHealth += health;
                        tabCard.Value.Health += health;
                        if (!isPermanent) affectedCards.Add(tabCard.Value);
                    }
                }
            }
            if (!affectedCards.Contains(null) && affectedCards != null && affectedCards.Count != 0) _tableAffectedCards[card] = new List<Card>(affectedCards);
            else _tableAffectedCards[card] = new List<Card>();
            affectedCards.Clear();
            _tableEffects[card] = (BuffEffect)card.Effect;
        }

        public void AllRestoreHealth(Card card, int health)
        {
            RestoreHealthHero(card.Player, health);
            switch (card.Player)
            {
                case PlayerType.Player1:
                    foreach(var _card in _table1Cards.Values)
                    {
                        if (_card == null) continue;
                        RestoreHealth(_card, health);
                    }
                    break;
                case PlayerType.Player2:
                    foreach (var _card in _table2Cards.Values)
                    {
                        if (_card == null) continue;
                        RestoreHealth(_card, health);
                    }
                    break;
            }
        }

        public void RestoreHealth(Card card, int health)
        {
            card.Health += health;
            Debug.Log($"{card.Player}'s card \"{card.name}\" is healed by {health} points");
            if (card.Health > card.MaxHealth) 
            { 
                card.Health = card.MaxHealth;
                card.HealthColor = Color.white;
            }
        }

        public void RestoreHealthHero(PlayerType player, int health)
        {
            switch (player)
            {
                case PlayerType.Player1:
                    _player1Hero.Health += health;
                    Debug.Log($"Player 1 Hero is Healed by {health} points");
                    if (_player1Hero.Health > _player1Hero.MaxHealth) _player1Hero.Health = _player1Hero.MaxHealth;
                    break;
                case PlayerType.Player2:
                    _player2Hero.Health += health;
                    Debug.Log($"Player 2 Hero is Healed by {health} points");
                    if (_player2Hero.Health > _player2Hero.MaxHealth) _player2Hero.Health = _player2Hero.MaxHealth;
                    break;
            }
        }

        public void SetTarget(Card target)
        {
            _target = target;
        }

        public void SetTarget(PlayerHero target)
        {
            _target = target;
        }

        public void StartChooseTarget(Card card, int damage, int health)
        {
            //to do
            StartCoroutine(ChooseTarget(card, damage, health));
        }

        private IEnumerator ChooseTarget(Card card, int damage, int health)
        {
            IsChoosingTarget = true;
            _effectHintWindow.SetActive(true);
            while (_target == null)
            {
                yield return null;
            }

            if (_target.GetType() == typeof(Card)) 
            {
                Card target = (Card)_target;
                var effectType = card.Effect.GetType();
                if (effectType == typeof(DealDamageEffect))
                {
                    if (card.Player != target.Player)
                    {
                        Debug.Log($"Dealing damage to {_target.name} enemy card ({damage} points)");
                        target.Health -= damage;
                        DamageTakenEvent?.Invoke(target);
                        CheckCardHealth(target);
                        _effectHintWindow.SetActive(false);
                        IsChoosingTarget = false;
                        _target = null;
                    }
                    else
                    {
                        _target = null;
                        Debug.LogWarning("This is not an enemy card. Choose another one.");
                        StartCoroutine(ChooseTarget(card, damage, health));
                    }
                }
                else if (effectType == typeof(RestoreHealthEffect))
                {
                    //RestoreHealth

                    if (card.Player == target.Player)
                    {
                        Debug.Log($"Restoring health to {_target.name} friendly card ({health} points)");
                        RestoreHealth(target, health);
                        _effectHintWindow.SetActive(false);
                        IsChoosingTarget = false;
                        _target = null;
                    }
                    else
                    {
                        _target = null;
                        Debug.LogWarning("This is not a friendly card. Choose another one.");
                        StartCoroutine(ChooseTarget(card, damage, health));
                    }
                }
                else if (effectType == typeof(BuffEffect))
                {
                    if (card.Player == target.Player)
                    {
                        Debug.Log($"Buffing {_target.name} friendly card");
                        target.Attack += damage;
                        target.MaxHealth += health;
                        target.Health += health;
                        _effectHintWindow.SetActive(false);
                        IsChoosingTarget = false;
                        _target = null;
                    }
                    else
                    {
                        _target = null;
                        Debug.LogWarning("This is not a friendly card. Choose another one.");
                        StartCoroutine(ChooseTarget(card, damage, health));
                    }
                }
            }
            else if(_target.GetType() == typeof(PlayerHero))
            {
                PlayerHero target = (PlayerHero)_target;
                var effectType = card.Effect.GetType();
                if (effectType == typeof(DealDamageEffect))
                {
                    if (card.Player != target.Player)
                    {
                        Debug.Log($"Dealing damage to {_target.name} (by {damage} points)");
                        target.Health -= damage;
                        _effectHintWindow.SetActive(false);
                        IsChoosingTarget = false;
                        _target = null;
                    }
                    else
                    {
                        _target = null;
                        Debug.LogWarning("This is not an enemy Hero. Choose another character.");
                        StartCoroutine(ChooseTarget(card, damage, health));
                    }
                }
                else if (effectType == typeof(RestoreHealthEffect))
                {
                    //RestoreHealth

                    if (card.Player == target.Player)
                    {
                        Debug.Log($"Restoring health to {_target.name} (by {health} points)");
                        RestoreHealthHero(target.Player, health);
                        _effectHintWindow.SetActive(false);
                        IsChoosingTarget = false;
                        _target = null;
                    }
                    else
                    {
                        _target = null;
                        Debug.LogWarning("This is not a friendly Hero. Choose another character.");
                        StartCoroutine(ChooseTarget(card, damage, health));
                    }
                }
                else
                {
                    _target = null;
                    Debug.LogWarning("This effect cannot be applied to Hero. Choose another character.");
                    StartCoroutine(ChooseTarget(card, damage, health));
                }
            }
            //IsChoosingTarget = false;
        }
    }
}