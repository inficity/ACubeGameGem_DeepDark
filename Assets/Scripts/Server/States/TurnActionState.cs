
namespace DeepDark.Server.States
{
	public class TurnActionState : State
	{
		public void start()
		{
			UnityEngine.Debug.Log("TurnActionState.start");
			StateManager.Instance.makeTransition<States.ReadyState>();
		}

		public void end()
		{

		}
	}
}