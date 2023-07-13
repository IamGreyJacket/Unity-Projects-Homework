using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        [HideInInspector]
        public Card[] Cards;

        [SerializeField]
        private Transform[] _handPositions;


        private void Start()
        {
            Cards = new Card[_handPositions.Length];
        }

        public bool SetNewCard(Card card)
        {
            int index = -1;
            for(int i = 0; i < Cards.Length; i++)
            {
                if(Cards[i] == null)
                {
                    index = i;
                    break;
                }
            }
            if(index == -1)
            {
                Destroy(card.gameObject);
                return false;
            }

            Cards[index] = card;
            StartCoroutine(MoveInHand(card, _handPositions[index]));
            return true;
        }

        private IEnumerator MoveInHand(Card card, Transform position)
        {
            var time = 0f;
            var startPos = card.transform.position;
            var endPos = card.transform.position + new Vector3(0f, 50f, 0f);
            while (time < .5f)
            {
                card.transform.position = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime;
                yield return null;
            }
            card.SwitchCard();
            yield return new WaitForSeconds(1f);

            time = 0f;
            startPos = card.transform.position;
            endPos = position.position;
            while (time < 1f)
            {
                card.transform.position = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime;
                yield return null;
            }
            card.State = CardStateType.InHand;
        }

        public void SwitchCards()
        {
            foreach (var card in Cards)
            {
                if (card == null) continue;
                card.SwitchCard();
            }
        }

        public void RotateCards()
        {
            foreach(var card in Cards)
            {
                if (card == null) continue;
                card.StartRotateCard();
            }
        }
    }
}