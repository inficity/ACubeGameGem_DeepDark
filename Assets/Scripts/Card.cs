
using UnityEngine;

namespace DeepDark
{
	public enum CardType {
		Character,
		Magic,

	}
	public class Card
	{
		public int Id { get; private set; }
		public bool IsNegative { get; private set; }
		public CardType CardType { get; private set; }
		public string Name { get; private set; }
		public int Cost { get; private set; }
		public string Description { get; private set; }
		public int Power { get; private set; }
		public int HP { get; private set; }
	}

	namespace Client
	{
		public class PlayCard : MonoBehaviour
		{
			public Card Card;
			public Texture2D Image { get; private set; }
			
			// Update is called once per frame
			void Update () {
				
			}

			public void OnUse() {
				GameManager.Instance.UseCard(this);
			}
		}
	}

}