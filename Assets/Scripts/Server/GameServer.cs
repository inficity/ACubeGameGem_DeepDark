﻿
using UnityEngine.Networking;

namespace DeepDark.Server
{
	public class GameServer
	{
		public static GameServer Instance { get; private set; }

		public Global<PlayerGameSetting> GlobalPlayerGameSetting { get; private set; }
		public Global<PlayerGameState> GlobalPlayerGameState { get; private set; }

		private StateManager stateManager;

		public GameServer()
		{
			GameServer.Instance = this;

			this.GlobalPlayerGameSetting = new Global<PlayerGameSetting>(
				new PlayerGameSetting(
					hp:					30,
					cost:				0,
					initNegativeHand:	3,
					initPositiveHand:	3,
					maxNegativeHand:	3,
					maxPositiveHand:	3),

				new PlayerGameSetting(
					hp:					30,
					cost:				0,
					initNegativeHand:	3,
					initPositiveHand:	3,
					maxNegativeHand:	3,
					maxPositiveHand:	3));

			this.stateManager = new StateManager();
			this.stateManager.makeTransition<States.ReadyState>();

			NetworkServer.Configure(new ConnectionConfig(), 2);
			NetworkServer.Listen(8888);
		}
	}
}