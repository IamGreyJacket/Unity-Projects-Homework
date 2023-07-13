using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Restore health effect", fileName = "NewRestoreHealthEffect")]
	public class RestoreHealthEffect : EffectData
	{
		public EffectTargetType Target;
		[field: SerializeField]
		public int Health;
		public override void Apply(Card card)
		{
            //Восстанавливает здоровье либо герою, либо определенной цели, либо всем, не выходя за пределы максимального здоровья
            switch (Target)
            {
                case EffectTargetType.AllCardAndHero:
                    GameManager.Self.TableManager.AllRestoreHealth(card, Health);
                    break;
                case EffectTargetType.Card:
                    //to do
                    GameManager.Self.TableManager.StartChooseTarget(card, 0, Health);
                    
                    break;
            }
		}

        public override void Discard(Card card)
        {
            
        }
    }
}

