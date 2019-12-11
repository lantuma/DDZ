namespace ETModel
{
	[ObjectSystem]
	public class PlayerSystem1 : AwakeSystem<Player1, string>
	{
		public override void Awake(Player1 self, string a)
		{
			self.Awake(a);
		}
	}

	public sealed class Player1 : Entity
	{
		public string Account { get; private set; }
		
		public long UnitId { get; set; }

		public void Awake(string account)
		{
			this.Account = account;
		}
		
		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
		}
	}
}