
namespace ETModel
{
    [ObjectSystem]
    public class UserAwakeSystem : AwakeSystem<User, long>
    {
        public override void Awake(User self, long id)
        {
            self.Awake(id);
        }
    }

    public class User:Entity
    {
        //用户ID（唯一）
        public long UserID { get; private set; }

        public void Awake(long id)
        {
            this.UserID = id;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.UserID = 0;
        }
    }
}
