
using System.Collections.Generic;

using UnityEngine.Networking;

namespace DeepDark.Server.States
{
	public class ReadyState : State
	{
		private Dictionary<int, bool> readyMap = new Dictionary<int, bool>();

		public void start()
		{
			NetworkServer.RegisterHandler(MsgType.Connect, this.__handle_CONNECT);
			NetworkServer.RegisterHandler(MsgType.Disconnect, this.__handle_DISCONNECT);
			NetworkServer.RegisterHandler(Messages.Type.READY, this.__handle_READY);
		}

		public void end()
		{
			NetworkServer.UnregisterHandler(MsgType.Connect);
			NetworkServer.UnregisterHandler(MsgType.Disconnect);
			NetworkServer.UnregisterHandler(Messages.Type.READY);
		}

		private void __handle_CONNECT(NetworkMessage networkMessage)
		{
			this.readyMap.Add(networkMessage.conn.connectionId, false);
		}

		private void __handle_DISCONNECT(NetworkMessage networkMessage)
		{
			this.readyMap.Remove(networkMessage.conn.connectionId);
		}

		private void __handle_READY(NetworkMessage networkMessage)
		{
			this.readyMap[networkMessage.conn.connectionId] = true;

			if (this.readyMap.Count != 2)
				return;

			foreach (var pair in this.readyMap)
				if (!pair.Value)
					return;

			StateManager.Instance.makeTransition<GameStartState>();
		}
	}
}