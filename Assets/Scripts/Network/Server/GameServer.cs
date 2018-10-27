
namespace DeepDark.Network.Server
{
	public class GameServer
	{
		public Global<PlayerGameSetting> GlobalPlayerGameSetting { get; private set; }

		public GameServer()
		{
			this.GlobalPlayerGameSetting = new Global<PlayerGameSetting>(
				new PlayerGameSetting(
					hp:					30,
					cost:				0,
					initNegativeHand:	3,
					initPositiveHand:	3,
					maxNegativeHand:	3,
					maxPositiveHand:	3),

				new PlayerGameSetting(
					hp: 30,
					cost: 0,
					initNegativeHand: 3,
					initPositiveHand: 3,
					maxNegativeHand: 3,
					maxPositiveHand: 3));
		}
	}
}