
using System;

namespace DeepDark
{
	public enum CardType {
		Character,
		Magic,

	}

	public class Card
	{
		public int Id { get; set; }
		public bool IsNegative { get; set; }
		public CardType CardType { get; set; }
		public string Name { get; set; }
		public int Cost { get; set; }
		public string Description { get; set; }
		public string Effect { get; set; }
		public int Power { get; set; }
		public int HP { get; set; }

		public Action<PlayerGameState, PlayerGameState> OnUseCard;
		public Action<Server.ServerCharacter, PlayerGameState, PlayerGameState> OnBeginTurn;
	}
}
