using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneLine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Summon effect", fileName = "NewSummonEffect")]
	public class SummonEffect : EffectData
	{
		[SerializeField, OneLine(Header = LineHeader.Short)]
		public CardPropertiesData CardData;
		public override void Apply(Card card)
		{
			var temp = Instantiate(card.gameObject);
			temp.transform.localScale = card.StartScale;
			Card newCard = temp.GetComponent<Card>();
			GameManager.Self.TableManager.SummonCard(CardData, newCard, card.Player);
		}

        public override void Discard(Card card)
        {
			
        }
    }
}
