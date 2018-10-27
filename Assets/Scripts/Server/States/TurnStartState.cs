
using System.Collections.Generic;

using UnityEngine.Networking;

namespace DeepDark.Server.States
{
	public class TurnStartState : State
	{
		public void start()
		{
			UnityEngine.Debug.Log("TurnStartState.start");

			NetworkServer.RegisterHandler(MsgType.Disconnect, this.__handle_DISCONNECT);

			var firstPair = GameServer.Instance.GlobalPlayerGameState.Map[GameServer.Instance.FirstId].fillHand(GameServer.Instance.GlobalPlayerGameSetting.Map[GameServer.Instance.FirstId]);
			var secondPair = GameServer.Instance.GlobalPlayerGameState.Map[GameServer.Instance.SecondId].fillHand(GameServer.Instance.GlobalPlayerGameSetting.Map[GameServer.Instance.SecondId]);
			var currentTurnId = GameServer.Instance.GlobalPlayerGameState.Map[GameServer.Instance.FirstId].Turn ? GameServer.Instance.FirstId : GameServer.Instance.SecondId;

			foreach (var serverCharacter in GameServer.Instance.GlobalPlayerGameState.Map[currentTurnId].Field)
				serverCharacter.onTurnBegin(
					GameServer.Instance.GlobalPlayerGameState.Map[currentTurnId],
					GameServer.Instance.GlobalPlayerGameState.Map[currentTurnId == GameServer.Instance.FirstId ? GameServer.Instance.SecondId : GameServer.Instance.FirstId]);

			TurnStartState.__sendMessage(currentTurnId, GameServer.Instance.FirstId, firstPair.Key, firstPair.Value, secondPair.Key, secondPair.Value);
			TurnStartState.__sendMessage(currentTurnId, GameServer.Instance.SecondId, secondPair.Key, secondPair.Value, firstPair.Key, firstPair.Value);

			StateManager.Instance.makeTransition<States.TurnActionState>();
		}

		public void end()
		{
			NetworkServer.UnregisterHandler(MsgType.Disconnect);
		}

		private void __handle_DISCONNECT(NetworkMessage networkMessage)
		{
			var message = new Messages.GameEndMessage();
			message.winner = GameServer.Instance.FirstId == networkMessage.conn.connectionId ? GameServer.Instance.SecondId : GameServer.Instance.FirstId;
			GameServer.Instance.sendMessage(Messages.Type.GAME_END, message);

			StateManager.Instance.makeTransition<States.ReadyState>();
		}

		private static void __sendMessage(int turnPlayerId, int playerId, List<int> negative, List<int> positive, List<int> enemyNegative, List<int> enemyPositive)
		{
			var message = new Messages.TurnStartMessage();
			message.clientId = turnPlayerId;
			message.timeout = GameServer.Instance.GlobalPlayerGameSetting.Map[turnPlayerId].TurnTimeout;
			message.negative = negative;
			message.positive = positive;
			message.enemyNegative = enemyNegative;
			message.enemyPositive = enemyPositive;

			GameServer.Instance.sendMessage(playerId, Messages.Type.TURN_START, message);
		}
	}
}