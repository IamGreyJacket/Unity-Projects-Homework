using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Draw card effect", fileName = "NewDrawCardEffect")]
	public class DrawCardEffect : EffectData
	{
		public override void Apply(Card card)
		{
			GameManager.Self.DeckManager.DrawCard();
		}

        public override void Discard(Card card)
        {
			
        }
    }
}
