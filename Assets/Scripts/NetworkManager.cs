
using DeepDark.Client;
using DeepDark.Server;

using UnityEngine;
using UniRx;
using TimeSpan = System.TimeSpan;

namespace DeepDark
{
	public class NetworkManager : MonoBehaviour, ClientEventHandler
	{
		public static NetworkManager Instance;
		private GameClient gameClient;
		private GameServer gameServer;

		void Awake()
		{
			Instance = this;
		}
		public void runServer()
		{
			this.gameServer = new GameServer();
			this.gameClient = new GameClient("127.0.0.1", 8888, this);
		}

		int connectTryIndex = 0;
		public void connectServer(string ip)
		{
			Debug.Log($"connectServer #{ip}");
			this.gameClient = new GameClient(ip, 8888, this);
			var connIndex = ++connectTryIndex;
		}

		public void onConnected()
		{
			Debug.Log("onConnected");
			NetworkUI.Instance.ShowConnectUI(false);
			NetworkUI.Instance.ShowReadyUI(true);
		}

		public void sendReady()
		{
			gameClient.sendMessage(Messages.Type.READY, new Messages.EmptyMessage());
		}

		public ScheduledNotifier<bool> onDisconnectedNotifier = new ScheduledNotifier<bool>();
		public void onDisconnected()
		{
			Debug.Log("onDisconnected");
		}

		public ScheduledNotifier<Messages.GameStartMessage> onGameStartedNotifier = new ScheduledNotifier<Messages.GameStartMessage>();
		public void onGameStarted(Messages.GameStartMessage message)
		{
			Debug.Log("onConGameStartMessagenected");
			onGameStartedNotifier.Report(message);
		}

		public ScheduledNotifier<Messages.TurnStartMessage> onTurnStartedNotifier = new ScheduledNotifier<Messages.TurnStartMessage>();
		public void onTurnStarted(Messages.TurnStartMessage message)
		{
			Debug.Log("TurnStartMessage");
			onTurnStartedNotifier.Report(message);
		}

		public ScheduledNotifier<Messages.TurnActionResponseMessage> onTurnActionRespondedNotifier = new ScheduledNotifier<Messages.TurnActionResponseMessage>();
		public void onTurnActionResponded(Messages.TurnActionResponseMessage message)
		{
			Debug.Log("TurnActionResponseMessage");
			onTurnActionRespondedNotifier.Report(message);
		}

		public ScheduledNotifier<Messages.TurnActionEventMessage> onTurnActionEventNotifier = new ScheduledNotifier<Messages.TurnActionEventMessage>();
		public void onTurnActionEvent(Messages.TurnActionEventMessage message)
		{
			Debug.Log("TurnActionEventMessage");
			onTurnActionEventNotifier.Report(message);
		}

		public ScheduledNotifier<Messages.GameEndMessage> onGameEndedNotifier = new ScheduledNotifier<Messages.GameEndMessage>();
		public void onGameEnded(Messages.GameEndMessage message)
		{
			Debug.Log("GameEndMessage");
			onGameEndedNotifier.Report(message);
		}
	}
}