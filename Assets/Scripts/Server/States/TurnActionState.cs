
using UnityEngine.Networking;

namespace DeepDark.Server.States
{
	public class TurnActionState : State
	{
		public void start()
		{
			UnityEngine.Debug.Log("TurnActionState.start");

			NetworkServer.RegisterHandler(MsgType.Disconnect, this.__handle_DISCONNECT);

			StateManager.Instance.makeTransition<States.ReadyState>();
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
	}
}