using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Spell damage effect", fileName = "NewSpellDamageEffect")]
	public class SpellDamageEffect : EffectData
	{
		[field: SerializeField]
		public int SpellDamage;
		public override void Apply(Card card)
		{
			//Увеличивает урон заклинаний на время существования карты.
		}

        public override void Discard(Card card)
        {
            //to do
        }
    }
}
