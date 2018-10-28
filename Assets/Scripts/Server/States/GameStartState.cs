
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

			GameStartState.__sendMessage(GameServer.Instance.FirstId);
			GameStartState.__sendMessage(GameServer.Instance.SecondId);

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

		private static void __sendMessage(int playerId)
		{
			var state = GameServer.Instance.GlobalPlayerGameState.Map[playerId];
			var enemyState = GameServer.Instance.GlobalPlayerGameState.Map[GameServer.Instance.FirstId == playerId ? GameServer.Instance.SecondId : GameServer.Instance.FirstId];
			var message = new Messages.GameStartMessage();

			message.clientId = playerId;
			message.faceId = Random.Range(0, 5);
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