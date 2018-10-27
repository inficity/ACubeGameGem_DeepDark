
namespace DeepDark.Client
{
	public interface ClientEventHandler
	{
		void onConnected();
		void onDisconnected();
		void onGameStarted(Messages.GameStartMessage message);
		void onTurnStarted(Messages.TurnStartMessage message);
		void onTurnActionResponded(Messages.TurnActionResponseMessage message);
		void onTurnActionEvent(Messages.TurnActionEventMessage message);
		void onGameEnded(Messages.GameEndMessage message);
	}
}