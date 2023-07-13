using RotaryHeart.Lib.SerializableDictionary;

namespace AI
{
	public enum AnimationType : byte
	{
		Move = 0,
		FastAttack = 1,
		StrongAttack = 2,
		Die = 3
	}

	[System.Serializable]
	public class AnimationKeyDictionary : SerializableDictionaryBase<AnimationType, string> { }
}
