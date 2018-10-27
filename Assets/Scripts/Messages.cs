
using Newtonsoft.Json;

using System.Collections.Generic;

using UnityEngine.Networking;

namespace DeepDark
{
	public static class Messages
	{
		public static class Type
		{
			private const int BASE = 1000;

			public const int READY = BASE + 01;
			public const int GAME_START = BASE + 02;
			public const int TURN_START = BASE + 03;
			public const int TURN_ACTION = BASE + 04;
			public const int TURN_ACTION_RESPONSE = BASE + 05;
			public const int TURN_ACTION_EVENT = BASE + 06;
			public const int GAME_END = BASE + 07;
		}

		public class EmptyMessage : MessageBase
		{
			//Empty.
		}

		public class JSONMessage : MessageBase
		{
			public string json;

			public void from<T>(T t)
			{
				this.json = JsonConvert.SerializeObject(t);
			}

			public T to<T>()
			{
				return JsonConvert.DeserializeObject<T>(this.json);
			}
		}

		public class GameStartMessage
		{
			public int clientId;
			public bool firstTurn;

			public int hp;
			public int cost;
			public List<int> negativeHand;
			public List<int> positiveHand;

			public int enemyHP;
			public int enemyCost;
			public List<int> enemyNegativeHand;
			public List<int> enemyPositiveHand;
		}

		public class TurnStartMessage
		{
			public int clientId;
			public int timeout;
		}

		public class TurnActionMessage
		{
			public TurnAction turnAction;

			//Attack
			public int damagerInstanceId;
			public int damageeInstanceId;

			//UseCard
			public int cardId;

			//TurnEnd
			//Empty.
		}

		public class TurnActionResponseMessage
		{
			public bool approved;
		}

		public class TurnActionEventMessage
		{
			public TurnActionEvent turnActionEvent;

			//HPChanged
			public int clientId;
			public int amount;

			//CharacterHPChanged
			public int instanceId;
			///public int amount;

			//Instantiated
			public int cardId;
			///public int instanceId;

			//Destroyed
			///public int instanceId;

			//BuffAttached
			///public int clientId;
			public int buffId;
			public int buffInstanceId;

			//BuffRemoved
			///public int clientId;
			///public int buffInstanceId;
		}

		public class GameEndMessage
		{
			public int winner;
		}
	}
}