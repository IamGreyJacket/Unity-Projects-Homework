using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Deal damage effect", fileName = "NewDealDamageEffect")]
	public class DealDamageEffect : EffectData
	{
		public EffectTargetType Target;
		[field: SerializeField]
		public int Damage;
		public override void Apply(Card card)
		{
            //to do
            switch (Target)
            {
                case EffectTargetType.Card:
                    //to do
                    GameManager.Self.TableManager.StartChooseTarget(card, Damage, 0);
                    break;
                case EffectTargetType.Hero:
                    if (card.Player == PlayerType.Player1)
                    {
                        GameManager.Self.PunishHero(PlayerType.Player2, Damage);
                    }
                    if (card.Player == PlayerType.Player2)
                    {
                        GameManager.Self.PunishHero(PlayerType.Player1, Damage);
                    }
                    break;
            }
		}

        public override void Discard(Card card)
        {
            
        }
    }
}
