using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Unique effect", fileName = "NewUniqueEffect")]
	public class UniqueEffect : EffectData
	{
		[field: SerializeField]
		public int Attack;

		private Card _thisCard;

		public override void Apply(Card card)
		{
			//to do
			_thisCard = card;
			GameManager.Self.TableManager.DamageTakenEvent += BuffCard;
		}

		public void BuffCard(Card card)
        {
			if(card == _thisCard)
            {
				card.Attack += Attack;
            }
        }

		public override void Discard(Card card)
		{
			//To do
		}

        private void OnDisable()
        {
			GameManager.Self.TableManager.DamageTakenEvent -= BuffCard;
			_thisCard = null;
		}
    }
}
