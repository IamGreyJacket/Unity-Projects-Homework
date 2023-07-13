using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneLine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Concrete effect", fileName = "NewConcreteEffect")]
	public class ConcreteEffect : EffectData
	{
		[field: SerializeField]
		public float Somevalue { get; private set; }
		[SerializeField, OneLine(Header = LineHeader.Short)]
		public CardPropertiesData Cards;
		[field: SerializeField]
		public EffectTargetType EffectType { get; private set; }
		[field: SerializeField]
		public float Action { get; private set; }

		public override void Apply(Card card)
		{
			Debug.Log("Do something(with your life)");
		}

        public override void Discard(Card card)
        {
            
        }
    }
}
