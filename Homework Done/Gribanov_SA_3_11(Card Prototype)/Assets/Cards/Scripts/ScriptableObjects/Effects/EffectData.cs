using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	public abstract class EffectData : ScriptableObject
	{
		[field: SerializeField] public string Description { get; private set; }
		public abstract void Apply(Card card);
		public abstract void Discard(Card card);
	}
}