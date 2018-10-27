
using UnityEngine;
using UnityEngine.Networking;

namespace DeepDark.Server
{
	public class GameServer
	{
		public static GameServer Instance { get; private set; }

		public int FirstId { get; private set; }
		public int SecondId { get; private set; }
		public Global<PlayerGameSetting> GlobalPlayerGameSetting { get; private set; }
		public Global<PlayerGameState> GlobalPlayerGameState { get; private set; }

		private StateManager stateManager;

		public GameServer()
		{
			GameServer.Instance = this;

			this.stateManager = new StateManager();
			this.stateManager.makeTransition<States.ReadyState>();

			NetworkServer.Listen(8888);
		}

		public void sendMessage<T>(short messageType, T message)
		{
			var json = new Messages.JSONMessage();
			json.from(message);

			NetworkServer.SendToAll(messageType, json);
		}

		public void sendMessage<T>(int playerId, short messageType, T message)
		{
			var json = new Messages.JSONMessage();
			json.from(message);

			NetworkServer.SendToClient(playerId, messageType, json);
		}

		public void readyPlayer(int firstId, int secondId)
		{
			this.FirstId = firstId;
			this.SecondId = secondId;
		}

		public void startGame()
		{
			this.GlobalPlayerGameSetting = new Global<PlayerGameSetting>(
				this.FirstId,
				this.SecondId,
				new PlayerGameSetting(
					hp: 29,
					cost: 0,
					turnTimeout: 30,
					initNegativeHand: 3,
					initPositiveHand: 3,
					maxNegativeHand: 3,
					maxPositiveHand: 3),

				new PlayerGameSetting(
					hp: 29,
					cost: 0,
					turnTimeout: 30,
					initNegativeHand: 3,
					initPositiveHand: 3,
					maxNegativeHand: 3,
					maxPositiveHand: 3));

			var firstFirst = Random.Range(0, 2) == 1;

			this.GlobalPlayerGameState = new Global<PlayerGameState>(
				this.FirstId,
				this.SecondId,
				new PlayerGameState(this.FirstId, firstFirst, this.GlobalPlayerGameSetting.Map[this.FirstId]),
				new PlayerGameState(this.SecondId, !firstFirst, this.GlobalPlayerGameSetting.Map[this.SecondId]));
		}
	}
}