
using System.Collections.Generic;

using UnityEngine.Networking;

namespace DeepDark.Server.States
{
	public class GameStartState : State
	{
		public void start()
		{
			StateManager.Instance.makeTransition<ReadyState>();
		}

		public void end()
		{
			//Empty.
		}
	}
}