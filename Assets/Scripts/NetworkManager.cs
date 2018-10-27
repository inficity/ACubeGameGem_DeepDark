
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

		public void connectServer(string ip)
		{
			Debug.Log($"connectServer #{ip}");
			this.gameClient = new GameClient(ip, 8888, this);
		}

		public ScheduledNotifier<bool> onConnectionNotifier = new ScheduledNotifier<bool>();
		public void onConnected()
		{
			Debug.Log("onConnected");
			onConnectionNotifier.Report(true);
		}

		public void sendReady()
		{
			Debug.Log("send READY");
			gameClient.sendMessage(Messages.Type.READY, new Messages.EmptyMessage());
		}

		public void sendUseCard(int id)
		{
			Debug.Log($"send UseCard {id}");
			var msg = new Messages.TurnActionMessage();
			msg.clientId = clientId;
			msg.turnAction = TurnAction.UseCard;
			msg.id = id;
			gameClient.sendMessage(Messages.Type.TURN_ACTION, msg);
		}

		public void onDisconnected()
		{
			Debug.Log("onDisconnected");
			onConnectionNotifier.Report(true);
		}

		int clientId;
		public ScheduledNotifier<Messages.GameStartMessage> onGameStartedNotifier = new ScheduledNotifier<Messages.GameStartMessage>();
		public void onGameStarted(Messages.GameStartMessage message)
		{
			Debug.Log("onGameStarted");
			onGameStartedNotifier.Report(message);
			clientId = message.clientId;
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