
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

namespace DeepDark.Server.States
{
	public class GameStartState : State
	{
		public void start()
		{
			UnityEngine.Debug.Log("GameStartState.start");

			NetworkServer.RegisterHandler(MsgType.Disconnect, this.__handle_DISCONNECT);
			GameServer.Instance.startGame();

			var firstFaceId = Random.Range(0, 5);
			var secondFaceId = Random.Range(0, 5);

			while (firstFaceId == secondFaceId)
				secondFaceId = Random.Range(0, 5);

			GameStartState.__sendMessage(GameServer.Instance.FirstId, firstFaceId, secondFaceId);
			GameStartState.__sendMessage(GameServer.Instance.SecondId, secondFaceId, firstFaceId);

			StateManager.Instance.makeTransition<States.TurnStartState>();
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

		private static void __sendMessage(int playerId, int faceId, int enemyFaceId)
		{
			var state = GameServer.Instance.GlobalPlayerGameState.Map[playerId];
			var enemyState = GameServer.Instance.GlobalPlayerGameState.Map[GameServer.Instance.FirstId == playerId ? GameServer.Instance.SecondId : GameServer.Instance.FirstId];
			var message = new Messages.GameStartMessage();

			message.faceId = faceId;
			message.enemyFaceId = enemyFaceId;

			message.clientId = playerId;
			message.firstTurn = state.Turn;

			message.hp = state.HP;
			message.cost = state.Cost;
			message.negativeHand = state.NegativeHand.Select(card => card.Id).ToList();
			message.positiveHand = state.PositiveHand.Select(card => card.Id).ToList();

			message.enemyHP = enemyState.HP;
			message.enemyCost = enemyState.Cost;
			message.enemyNegativeHand = enemyState.NegativeHand.Select(card => card.Id).ToList();
			message.enemyPositiveHand = enemyState.PositiveHand.Select(card => card.Id).ToList();

			GameServer.Instance.sendMessage(playerId, Messages.Type.GAME_START, message);
		}
	}
}