
using DeepDark.Network.Server;

using System.Collections.Generic;

namespace DeepDark
{
	public class PlayerGameState
	{
		public bool Turn { get; private set; }
		public int HP { get; private set; }
		public int Cost { get; private set; }
		public Queue<Card> NegativeDeck { get; private set; }
		public Queue<Card> PositiveDeck { get; private set; }
		public List<Card> NegativeHand { get; private set; }
		public List<Card> PositiveHand { get; private set; }

		public PlayerGameState(bool turn, PlayerGameSetting playerGameSetting)
		{
			this.Turn = turn;
			this.HP = playerGameSetting.HP;
			this.Cost = playerGameSetting.Cost;
			this.NegativeDeck = new Queue<Card>(playerGameSetting.NegativeDeck);
			this.PositiveDeck = new Queue<Card>(playerGameSetting.PositiveDeck);
			this.NegativeHand = new List<Card>();
			this.PositiveHand = new List<Card>();

			playerGameSetting.shuffleDeck();

			this.fillHand(playerGameSetting);
		}
		
		public void fillHand(PlayerGameSetting playerGameSetting)
		{
			PlayerGameState.__fillHand(this.NegativeDeck, this.NegativeHand, playerGameSetting.MaxNegativeHand);
			PlayerGameState.__fillHand(this.PositiveDeck, this.PositiveHand, playerGameSetting.MaxPositiveHand);
		}

		private static void __fillHand(Queue<Card> deck, List<Card> hand, int maxHand)
		{
			while (hand.Count < maxHand)
				hand.Add(deck.Dequeue());
		}

		public Card removeNegativeCard(int id)
		{
			return PlayerGameState.__removeCard(id, this.NegativeDeck, this.NegativeHand);
		}

		public Card removePositiveCard(int id)
		{
			return PlayerGameState.__removeCard(id, this.PositiveDeck, this.PositiveHand);
		}

		private static Card __removeCard(int id, Queue<Card> deck, List<Card> hand)
		{
			for (int index = 0; index < hand.Count; ++index)
				if (hand[index].Id == id)
				{
					Card card = hand[index];

					deck.Enqueue(card);
					hand.RemoveAt(index);

					return card;
				}

			return null;
		}
	}
}