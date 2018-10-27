
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
			Debug.Log($"connectServer #{ip}");
			this.gameClient = new GameClient(ip, 8888, this);
		}

		public void onConnected()
		{
			Debug.Log("onConnected");
			NetworkUI.Instance.ShowConnectUI(false);
			NetworkUI.Instance.ShowReadyUI(true);
		}

		public void onDisconnected()
		{
			Debug.Log("onDisconnected");
		}

		public void onGameStarted(Messages.GameStartMessage message)
		{
			Debug.Log("onConGameStartMessagenected");
		}

		public void onTurnStarted(Messages.TurnStartMessage message)
		{
			Debug.Log("TurnStartMessage");
		}

		public void onTurnActionResponded(Messages.TurnActionResponseMessage message)
		{
			Debug.Log("TurnActionResponseMessage");
		}

		public void onTurnActionEvent(Messages.TurnActionEventMessage message)
		{
			Debug.Log("TurnActionEventMessage");
		}

		public void onGameEnded(Messages.GameEndMessage message)
		{
			Debug.Log("GameEndMessage");
		}
	}
}