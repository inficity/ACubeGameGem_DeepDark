using UnityEngine;

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
		public int Power { get; set; }
		public int HP { get; set; }
	}
}