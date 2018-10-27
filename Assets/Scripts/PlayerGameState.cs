
using DeepDark.Server;

using System.Collections.Generic;

namespace DeepDark
{
	public class PlayerGameState
	{
		public int Id { get; private set; }
		public bool Turn { get; private set; }
		public int HP { get; private set; }
		public int Cost { get; private set; }
		public Queue<Card> NegativeDeck { get; private set; }
		public Queue<Card> PositiveDeck { get; private set; }
		public List<Card> NegativeHand { get; private set; }
		public List<Card> PositiveHand { get; private set; }
		public List<ServerCharacter> Field { get; private set; }

		public PlayerGameState(int id, bool turn, PlayerGameSetting playerGameSetting)
		{
			playerGameSetting.shuffleDeck();

			this.Id = id;
			this.Turn = turn;
			this.HP = playerGameSetting.HP;
			this.Cost = playerGameSetting.Cost;
			this.NegativeDeck = new Queue<Card>(playerGameSetting.NegativeDeck);
			this.PositiveDeck = new Queue<Card>(playerGameSetting.PositiveDeck);
			this.NegativeHand = new List<Card>();
			this.PositiveHand = new List<Card>();
			this.Field = new List<ServerCharacter>();

			this.fillHand(playerGameSetting);
		}

		public void swapTurn()
		{
			this.Turn = !this.Turn;
		}

		public void addHP(int amount)
		{
			this.HP += amount;
		}

		public void addCost(int amount)
		{
			this.Cost += amount;
		}

		public KeyValuePair<List<int>, List<int>> fillHand(PlayerGameSetting playerGameSetting)
		{
			return new KeyValuePair<List<int>, List<int>>(
				PlayerGameState.__fillHand(this.NegativeDeck, this.NegativeHand, playerGameSetting.MaxNegativeHand),
				PlayerGameState.__fillHand(this.PositiveDeck, this.PositiveHand, playerGameSetting.MaxPositiveHand));
		}

		private static List<int> __fillHand(Queue<Card> deck, List<Card> hand, int maxHand)
		{
			List<int> card = new List<int>();

			while (hand.Count < maxHand)
			{
				card.Add(deck.Peek().Id);
				hand.Add(deck.Dequeue());
			}

			return card;
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

		public ServerCharacter spawnCard(int cardId)
		{
			var serverCharacter = new ServerCharacter(CardManager.GetCard(cardId));

			this.Field.Add(serverCharacter);

			var message = new Messages.TurnActionEventMessage();
			message.turnActionEvent = TurnActionEvent.Instantiated;
			message.playerId = this.Id;
			message.cardId = cardId;
			message.instanceId = serverCharacter.Id;
			message.hp = serverCharacter.HP;
			message.power = serverCharacter.Power;
			message.attack = serverCharacter.AttackChance;

			GameServer.Instance.sendMessage(Messages.Type.TURN_ACTION_EVENT, message);

			return serverCharacter;
		}

		public ServerCharacter findServerCharacter(int instanceId)
		{
			foreach (var serverCharacter in this.Field)
				if (serverCharacter.Id == instanceId)
					return serverCharacter;

			return null;
		}

		public void removeServerCharacter(ServerCharacter serverCharacter)
		{
			this.Field.Remove(serverCharacter);
		}
	}
}