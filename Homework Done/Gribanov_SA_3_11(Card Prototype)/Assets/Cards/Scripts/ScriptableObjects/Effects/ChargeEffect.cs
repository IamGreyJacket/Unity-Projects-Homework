using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Charge effect", fileName = "NewChargeEffect")]
	public class ChargeEffect : EffectData
	{
		public override void Apply(Card card)
		{
			card.CanAttack = true;
		}

        public override void Discard(Card card)
        {
            
        }
    }
}