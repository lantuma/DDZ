namespace ETModel
{
    public class SubGame:Entity
    {
        public long GameID { get; set; }

        public int Index { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.GameID = 0;

            this.Index = -1;
        }
    }
}
