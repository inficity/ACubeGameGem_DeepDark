
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

			switch (message.turnAction)
			{
				case TurnAction.Attack:

					//message.damagerInstanceId
					

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
			message.instanceId = serverCharacter.Id;

			GameServer.Instance.sendMessage(Messages.Type.TURN_ACTION_RESPONSE, message);
		}
	}
}