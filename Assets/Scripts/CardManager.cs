using System.Collections;
using System.Collections.Generic;

namespace DeepDark
{
	public class CardManager
	{
		static CardManager Instance;
		Dictionary<int, Card> Cards = new Dictionary<int, Card>();
		public CardManager()
		{
			var id = 0;
			{
				var card = new Card();
				card.Id = ++id;
				card.Name = "직장 선배";
				card.Description = "";
				card.Cost = 3;
				card.Power = 2;
				card.HP = 2;
				card.IsNegative = true;
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = ++id;
				card.Name = "꼰대 대리";
				card.Description = "";
				card.Cost = 3;
				card.Power = 2;
				card.HP = 2;
				card.IsNegative = true;
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = ++id;
				card.Name = "만년 과장";
				card.Description = "";
				card.Cost = 3;
				card.Power = 2;
				card.HP = 2;
				card.IsNegative = true;
				Cards.Add(card.Id, card);
			}


			{
				var card = new Card();
				card.Id = ++id;
				card.Name = "여자 친구";
				card.Description = "";
				card.Cost = 3;
				card.Power = 2;
				card.HP = 2;
				card.IsNegative = false;
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = ++id;
				card.Name = "소꼽 친구";
				card.Description = "";
				card.Cost = 3;
				card.Power = 2;
				card.HP = 2;
				card.IsNegative = false;
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = ++id;
				card.Name = "어머니";
				card.Description = "";
				card.Cost = 3;
				card.Power = 2;
				card.HP = 2;
				card.IsNegative = false;
				Cards.Add(card.Id, card);
			}
		}

		static public Card GetCard(int id)
		{
			if (Instance == null)
			{
				Instance = new CardManager();
			}
			Card value;
			if (Instance.Cards.TryGetValue(id, out value))
				return value;
			return null;
		}
	}
}
