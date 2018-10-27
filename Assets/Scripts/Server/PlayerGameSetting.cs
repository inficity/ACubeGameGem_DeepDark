
using System;
using System.Collections.Generic;

namespace DeepDark.Server
{
	public class PlayerGameSetting
	{
		public int HP { get; private set; }
		public int Cost { get; private set; }
		public int MaxNegativeHand { get; private set; }
		public int MaxPositiveHand { get; private set; }
		public List<Card> NegativeDeck { get; private set; }
		public List<Card> PositiveDeck { get; private set; }

		public PlayerGameSetting(
			int hp,
			int cost,
			int initNegativeHand,
			int initPositiveHand,
			int maxNegativeHand,
			int maxPositiveHand)
		{
			this.HP = hp;
			this.Cost = cost;
			this.MaxNegativeHand = maxNegativeHand;
			this.MaxPositiveHand = maxPositiveHand;

			if (this.MaxNegativeHand < 1)
				this.MaxNegativeHand = 1;

			if (this.MaxPositiveHand < 1)
				this.MaxPositiveHand = 1;
		}

		public void shuffleDeck()
		{
			PlayerGameSetting.__shuffleDeck(this.NegativeDeck);
			PlayerGameSetting.__shuffleDeck(this.PositiveDeck);
		}

		private static void __shuffleDeck(List<Card> deck)
		{
			Random rand = new Random();

			for (int n = deck.Count; n-- > 1;)
			{
				int m = rand.Next(n + 1);
				Card card = deck[m];
				deck[m] = deck[n];
				deck[n] = card;
			}
		}
	}
}