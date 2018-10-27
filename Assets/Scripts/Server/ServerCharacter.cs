
namespace DeepDark.Server
{
	public class ServerCharacter
	{
		private static int GlobalId = 0;

		public Card card { get; private set; }
		public int Id { get; private set; }
		public int HP { get; private set; }
		public int Power { get; private set; }
		public int AttackChance { get; private set; }

		public ServerCharacter(Card card)
		{
			this.card = card;
			this.Id = ServerCharacter.GlobalId++;
			this.HP = card.HP;
			this.Power = card.Power;
			this.AttackChance = 0;
		}

		public void setAttackChance(int attackChance)
		{
			this.AttackChance = attackChance;
		}

		public void onTurnBegin(PlayerGameState state, PlayerGameState enemyState)
		{
			if (this.card.OnBeginTurn != null)
				this.card.OnBeginTurn(this, state, enemyState);
		}

		public bool attack()
		{
			if (this.AttackChance < 1)
				return false;

			--this.AttackChance;

			return true;
		}

		public bool damagedBy(ServerCharacter serverCharacter)
		{
			if (!serverCharacter.attack())
				return false;

			this.HP -= serverCharacter.Power;

			return true;
		}
	}
}