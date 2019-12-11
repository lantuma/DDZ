using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ETModel
{
    [ObjectSystem]
    public class DBQueryPaginationQueryAwakeSystem : AwakeSystem<DBQueryPaginationQuery, string, string, ETTaskCompletionSource<List<ComponentWithId>>>
    {
        public override void Awake(DBQueryPaginationQuery self, string collectionName,string page, ETTaskCompletionSource<List<ComponentWithId>> tcs)
        {
            self.CollectionName = collectionName;
            self.page = int.Parse(page.Split('|')[0]);
            self.dbsize = int.Parse(page.Split('|')[1]);
            self.Tcs = tcs;
        }
    }

    public sealed class DBQueryPaginationQuery : DBTask
    {
        public string CollectionName { get; set; }

        public int page { get; set; }

        public int dbsize { get; set; }

        public ETTaskCompletionSource<List<ComponentWithId>> Tcs { get; set; }

        public override async ETTask Run()
        {
            DBComponent dbComponent = Game.Scene.GetComponent<DBComponent>();

            List<ComponentWithId> components = new List<ComponentWithId>();

            try
            {
                FilterDefinitionBuilder<ComponentWithId> builderFilter = Builders<ComponentWithId>.Filter;
                components = dbComponent.GetCollection(this.CollectionName).Find(builderFilter.Empty).Skip(page).Limit(dbsize).ToList();
                await Task.Delay(0);
                this.Tcs.SetResult(components);
            }
            catch (Exception e)
            {
                this.Tcs.SetException(new Exception($"查询数据库异常! {CollectionName} {Id}", e));
            }
        }
    }
}
