
using System;
using System.Collections.Generic;

namespace DeepDark.Server
{
	public class Buff
	{
		public string Name { get; private set; }
		public int Duration { get; private set; }
		public HashSet<string> Tag { get; private set; }
		public Action<PlayerGameState, PlayerGameState> OnBeginTurn { get; private set; }

		public Buff(string name, int duration)
		{
			this.Name = name;
			this.Duration = duration;
			this.Tag = new HashSet<string>();
		}

		public void addTag(string tag)
		{
			this.Tag.Add(tag);
		}

		public bool haveTag(string tag)
		{
			return this.Tag.Contains(tag);
		}

		public void onBeginTurn(Action<PlayerGameState, PlayerGameState> onBeginTurn)
		{
			this.OnBeginTurn = onBeginTurn;
		}

		public bool ended(int playerId)
		{
			if (this.Duration < 1)
			{
				var message = new Messages.TurnActionEventMessage();
				message.turnActionEvent = TurnActionEvent.BuffRemoved;
				message.playerId = playerId;
				message.buffName = this.Name;

				GameServer.Instance.sendMessage(Messages.Type.TURN_ACTION_EVENT, message);

				return true;
			}

			--this.Duration;

			return false;
		}
	}
}