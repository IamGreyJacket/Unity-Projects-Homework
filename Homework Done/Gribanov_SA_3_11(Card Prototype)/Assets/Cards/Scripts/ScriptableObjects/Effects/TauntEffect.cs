using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	[CreateAssetMenu(menuName = "Effects/Taunt effect", fileName = "NewTauntEffect")]
	public class TauntEffect: EffectData
	{
		public override void Apply(Card card)
		{
			card.IsTaunt = true;
			if(card.Player == PlayerType.Player1) GameManager.Self.TableManager.TauntCardsCountPlayer1++;
			if(card.Player == PlayerType.Player2) GameManager.Self.TableManager.TauntCardsCountPlayer2++;
			//Как нибудь добавить карту в список внутри TableManager List<Card> TauntCards
		}

        public override void Discard(Card card)
        {
			//to do
			if (card.Player == PlayerType.Player1) GameManager.Self.TableManager.TauntCardsCountPlayer1--;
			if (card.Player == PlayerType.Player2) GameManager.Self.TableManager.TauntCardsCountPlayer2--;
			card.IsTaunt = false;
		}
    }
}