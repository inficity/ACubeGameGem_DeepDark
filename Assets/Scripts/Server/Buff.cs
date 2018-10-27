
using System;
using System.Collections.Generic;

namespace DeepDark.Server
{
	public class Buff
	{
		public int Duration { get; private set; }
		public HashSet<string> Tag { get; private set; }
		public Action<PlayerGameState, PlayerGameState> OnBeginTurn { get; private set; }

		public Buff(int duration)
		{
			this.Duration = duration;
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
	}
}