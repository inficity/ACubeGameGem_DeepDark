
using DeepDark.Server;

using System.Collections.Generic;

using UnityEngine;

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
		public List<Buff> BuffList { get; private set; }

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
			this.BuffList = new List<Buff>();

			this.fillHand(playerGameSetting);
		}

		public void swapTurn()
		{
			this.Turn = !this.Turn;
		}

		public void addHP(int amount)
		{
			if (this.haveTag("+30%") && amount < 0)
				amount = (int)(amount * 1.3);

			if (this.haveTag("invincible"))
				amount = 0;

			this.HP += amount;
		}

		public void addCost(int amount)
		{
			this.Cost += amount;
		}

		public void sendChangedMessage()
		{
			{
				var message = new Messages.TurnActionEventMessage();
				message.turnActionEvent = TurnActionEvent.StateChanged;
				message.playerId = this.Id;
				message.hp = this.HP;
				message.cost = this.Cost;

				GameServer.Instance.sendMessage(Messages.Type.TURN_ACTION_EVENT, message);
			}

			if (this.HP < 1)
			{
				var message = new Messages.GameEndMessage();
				message.winner = GameServer.Instance.FirstId == this.Id ? GameServer.Instance.SecondId : GameServer.Instance.FirstId;

				GameServer.Instance.sendMessage(Messages.Type.GAME_END, message);
				StateManager.Instance.makeTransition<Server.States.ReadyState>();
			}
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

		public ServerCharacter spawnCard(ServerCharacter serverCharacter)
		{
			this.Field.Add(serverCharacter);

			var message = new Messages.TurnActionEventMessage();
			message.turnActionEvent = TurnActionEvent.Instantiated;
			message.playerId = this.Id;
			message.cardId = serverCharacter.card.Id;
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

		public ServerCharacter destroyServerCharacterRandom()
		{
			var index = Random.Range(0, this.Field.Count);
			var serverCharacter = this.Field[index];

			this.Field.RemoveAt(index);

			var message = new Messages.TurnActionEventMessage();
			message.turnActionEvent = TurnActionEvent.Destroyed;
			message.instanceId = serverCharacter.Id;

			GameServer.Instance.sendMessage(Messages.Type.TURN_ACTION_EVENT, message);

			return serverCharacter;
		}

		public void addBuff(Buff buff)
		{
			this.BuffList.Add(buff);

			var message = new Messages.TurnActionEventMessage();
			message.turnActionEvent = TurnActionEvent.BuffAttached;
			message.playerId = this.Id;
			message.buffName = buff.Name;

			GameServer.Instance.sendMessage(Messages.Type.TURN_ACTION_EVENT, message);
		}

		public bool haveTag(string tag)
		{
			foreach (var buff in this.BuffList)
				if (buff.haveTag(tag))
					return true;

			return false;
		}
	}
}