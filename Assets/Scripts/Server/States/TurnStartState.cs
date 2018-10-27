
using UnityEngine.Networking;

namespace DeepDark.Server.States
{
	public class TurnStartState : State
	{
		public void start()
		{
			UnityEngine.Debug.Log("TurnStartState.start");

			NetworkServer.RegisterHandler(MsgType.Disconnect, this.__handle_DISCONNECT);

			TurnStartState.__sendMessage(GameServer.Instance.FirstId);
			TurnStartState.__sendMessage(GameServer.Instance.SecondId);

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
			var message = new Messages.TurnStartMessage();
			message.clientId = playerId;
			message.timeout = GameServer.Instance.GlobalPlayerGameSetting.Map[playerId].TurnTimeout;

			GameServer.Instance.sendMessage(playerId, Messages.Type.TURN_START, message);
		}
	}
}