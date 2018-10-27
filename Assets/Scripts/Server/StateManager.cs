
namespace DeepDark.Server
{
	public class StateManager
	{
		public static StateManager Instance { get; private set; }

		private State state;

		public StateManager()
		{
			StateManager.Instance = this;
		}

		public void makeTransition<T>() where T : State, new()
		{
			if (this.state != null)
				this.state.end();

			this.state = new T();
			this.state.start();
		}
	}
}