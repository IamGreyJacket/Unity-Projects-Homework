using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Buff effect", fileName = "NewBuffEffect")]
	public class BuffEffect : EffectData
	{
		public bool IsPermanent; //to do
		public EffectTargetType Target;
		public CardUnitType Type;
		[field: SerializeField]
		public int Attack;
		[field: SerializeField]
		public int Health;
		public override void Apply(Card card)
		{
            switch (Target)
            {
				case EffectTargetType.AllCard:
					GameManager.Self.TableManager.BuffCards(card, Type, Attack, Health, IsPermanent);
                    break;
                case EffectTargetType.Card:
					GameManager.Self.TableManager.StartChooseTarget(card, Attack, Health);
					break;
				case EffectTargetType.Self:
					//to do проверка количества карт на столе
					int count = GameManager.Self.TableManager.CountCards(card.Player, Type) - 1;
					card.Attack += Attack * count;
					card.MaxHealth -= Health * count;
					card.Health += Health * count;
					break;
            }
		}

        public override void Discard(Card targetedCard)
        {
			//To do
			targetedCard.Attack -= Attack;
			targetedCard.Health -= Health;
			targetedCard.MaxHealth -= Health;
			if (targetedCard.Health <= 0) targetedCard.Health = 1;
        }
    }
}
