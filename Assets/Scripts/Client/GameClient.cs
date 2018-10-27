
using UnityEngine.Networking;

namespace DeepDark.Client
{
	public class GameClient
	{
		public NetworkClient Client { get; private set; }

		private ClientEventHandler clientEventHandler;

		public GameClient(string ip, int port, ClientEventHandler clientEventHandler)
		{
			this.clientEventHandler = clientEventHandler;

			this.Client = new NetworkClient();
			this.Client.RegisterHandler(MsgType.Connect, networkMessage => this.clientEventHandler.onConnected());
			this.Client.RegisterHandler(MsgType.Disconnect, networkMessage => this.clientEventHandler.onDisconnected());
			this.Client.RegisterHandler(Messages.Type.GAME_START, networkMessage => this.clientEventHandler.onGameStarted(networkMessage.ReadMessage<Messages.JSONMessage>().to<Messages.GameStartMessage>()));
			this.Client.RegisterHandler(Messages.Type.TURN_START, networkMessage => this.clientEventHandler.onTurnStarted(networkMessage.ReadMessage<Messages.JSONMessage>().to<Messages.TurnStartMessage>()));
			this.Client.RegisterHandler(Messages.Type.TURN_ACTION_RESPONSE, networkMessage => this.clientEventHandler.onTurnActionResponded(networkMessage.ReadMessage<Messages.JSONMessage>().to<Messages.TurnActionResponseMessage>()));
			this.Client.RegisterHandler(Messages.Type.TURN_ACTION_EVENT, networkMessage => this.clientEventHandler.onTurnActionEvent(networkMessage.ReadMessage<Messages.JSONMessage>().to<Messages.TurnActionEventMessage>()));
			this.Client.RegisterHandler(Messages.Type.GAME_END, networkMessage => this.clientEventHandler.onGameEnded(networkMessage.ReadMessage<Messages.JSONMessage>().to<Messages.GameEndMessage>()));

			this.Client.Connect(ip, port);
		}

		public void sendMessage<T>(short messageType, T message)
		{
			var json = new Messages.JSONMessage();
			json.from(message);

			this.Client.Send(messageType, json);
		}
	}
}