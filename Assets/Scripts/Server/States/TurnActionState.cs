
using UnityEngine.Networking;

namespace DeepDark.Server.States
{
	public class TurnActionState : State
	{
		private bool negativeUsed = false;

		public void start()
		{
			UnityEngine.Debug.Log("TurnActionState.start");

			NetworkServer.RegisterHandler(MsgType.Disconnect, this.__handle_DISCONNECT);
			NetworkServer.RegisterHandler(Messages.Type.TURN_ACTION, this.__handle_TURN_ACTION);
		}

		public void end()
		{
			NetworkServer.UnregisterHandler(MsgType.Disconnect);
			NetworkServer.UnregisterHandler(Messages.Type.TURN_ACTION);
		}

		private void handleTimeout()
		{

		}

		private void __handle_DISCONNECT(NetworkMessage networkMessage)
		{
			var message = new Messages.GameEndMessage();
			message.winner = GameServer.Instance.FirstId == networkMessage.conn.connectionId ? GameServer.Instance.SecondId : GameServer.Instance.FirstId;
			GameServer.Instance.sendMessage(Messages.Type.GAME_END, message);

			StateManager.Instance.makeTransition<States.ReadyState>();
		}

		private void __handle_TURN_ACTION(NetworkMessage networkMessage)
		{
			var message = networkMessage.ReadMessage<Messages.JSONMessage>().to<Messages.TurnActionMessage>();
			var state = GameServer.Instance.GlobalPlayerGameState.Map[networkMessage.conn.connectionId];
			var enemyState = GameServer.Instance.GlobalPlayerGameState.Map[GameServer.Instance.FirstId == networkMessage.conn.connectionId ? GameServer.Instance.SecondId : GameServer.Instance.FirstId];

			if (!state.Turn)
			{
				this.__sendResponseMessage(networkMessage.conn.connectionId, false);
				return;
			}

			switch (message.turnAction)
			{
				case TurnAction.AttackPlayer:
					{
						var damager = state.findServerCharacter(message.damagerInstanceId);

						if (!damager.attack())
						{
							this.__sendResponseMessage(networkMessage.conn.connectionId, false);
							return;
						}

						state.addHP(-damager.HP);
						this.__sendResponseMessage(networkMessage.conn.connectionId, true);

						this.__sendStateChangedMessage(networkMessage.conn.connectionId, state);
						this.__sendCharacterStateChangedMessage(damager);
					}
					break;
				case TurnAction.AttackCharacter:
					{
						var damager = state.findServerCharacter(message.damagerInstanceId);
						var damagee = enemyState.findServerCharacter(message.damageeInstanceId);

						if (!damagee.damagedBy(damager))
						{
							this.__sendResponseMessage(networkMessage.conn.connectionId, false);
							return;
						}

						this.__sendResponseMessage(networkMessage.conn.connectionId, true);

						this.__sendCharacterStateChangedMessage(damager);
						this.__sendCharacterStateChangedMessage(damagee);

						if (damagee.HP < 1)
							this.__sendDestroyedMessage(damagee);
					}
					break;
				case TurnAction.TurnEnd:

					if (!this.negativeUsed)
					{
						this.__sendResponseMessage(networkMessage.conn.connectionId, false);
						return;
					}

					this.__sendResponseMessage(networkMessage.conn.connectionId, true);

					GameServer.Instance.GlobalPlayerGameState.Map[GameServer.Instance.FirstId].swapTurn();
					GameServer.Instance.GlobalPlayerGameState.Map[GameServer.Instance.SecondId].swapTurn();

					StateManager.Instance.makeTransition<States.TurnStartState>();

					break;
				case TurnAction.UseCard:

					Card card = CardManager.GetCard(message.cardId);

					if (!card.IsNegative && !this.negativeUsed)
					{
						this.__sendResponseMessage(networkMessage.conn.connectionId, false);
						return;
					}

					if (card.IsNegative)
					{
						this.negativeUsed = true;
						state.addCost(card.Cost);
					}
					else if (card.Cost > state.Cost)
					{
						this.__sendResponseMessage(networkMessage.conn.connectionId, false);
						return;
					}
					else
						state.addCost(-card.Cost);

					this.__sendResponseMessage(networkMessage.conn.connectionId, true);
					this.__sendStateChangedMessage(networkMessage.conn.connectionId, state);

					if (card.OnUseCard != null)
						card.OnUseCard(state, enemyState);

					break;
			}
		}

		private void __sendResponseMessage(int playerId, bool approved)
		{
			var message = new Messages.TurnActionResponseMessage();
			message.approved = approved;

			GameServer.Instance.sendMessage(playerId, Messages.Type.TURN_ACTION_RESPONSE, message);
		}

		private void __sendDestroyedMessage(ServerCharacter serverCharacter)
		{
			var message = new Messages.TurnActionEventMessage();
			message.turnActionEvent = TurnActionEvent.Destroyed;
			message.instanceId = serverCharacter.Id;

			GameServer.Instance.sendMessage(Messages.Type.TURN_ACTION_EVENT, message);
		}

		private void __sendStateChangedMessage(int playerId, PlayerGameState state)
		{
			var message = new Messages.TurnActionEventMessage();
			message.turnActionEvent = TurnActionEvent.HPChanged;
			message.playerId = playerId;
			message.hp = state.HP;
			message.cost = state.Cost;

			GameServer.Instance.sendMessage(Messages.Type.TURN_ACTION_EVENT, message);
		}

		private void __sendCharacterStateChangedMessage(ServerCharacter serverCharacter)
		{
			var message = new Messages.TurnActionEventMessage();
			message.turnActionEvent = TurnActionEvent.CharacterStateChanged;
			message.instanceId = serverCharacter.Id;
			message.hp = serverCharacter.HP;
			message.attack = serverCharacter.AttackChance;

			GameServer.Instance.sendMessage(Messages.Type.TURN_ACTION_EVENT, message);
		}
	}
}