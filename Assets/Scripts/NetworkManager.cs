
using DeepDark.Client;
using DeepDark.Server;

using UnityEngine;

namespace DeepDark
{
	public class NetworkManager : MonoBehaviour, ClientEventHandler
	{
		private GameClient gameClient;
		private GameServer gameServer;

		public void runServer()
		{
			this.gameServer = new GameServer();
			this.gameClient = new GameClient("127.0.0.1", 8888, this);
		}

		public void connectServer(string ip)
		{
			this.gameClient = new GameClient(ip, 8888, this);
		}

		public void onConnected()
		{
			throw new System.NotImplementedException();
		}

		public void onDisconnected()
		{
			throw new System.NotImplementedException();
		}

		public void onGameStarted(Messages.GameStartMessage message)
		{
			throw new System.NotImplementedException();
		}

		public void onTurnStarted(Messages.TurnStartMessage message)
		{
			throw new System.NotImplementedException();
		}

		public void onTurnActionResponded(Messages.TurnActionResponseMessage message)
		{
			throw new System.NotImplementedException();
		}

		public void onTurnActionEvent(Messages.TurnActionEventMessage message)
		{
			throw new System.NotImplementedException();
		}

		public void onGameEnded(Messages.GameEndMessage message)
		{
			throw new System.NotImplementedException();
		}
	}
}