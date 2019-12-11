using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ETModel
{
    public  static class  DBHelper<T>where T: Entity
    {
        public static bool QureySingleTableAllData( string collectionName, List<T>  dataList)
        {
            try
            {
                var collection = Game.Scene.GetComponent<DBComponent>().database.GetCollection<T>(collectionName);

                var results = collection.FindAsync<T>(new BsonDocument()).Result;

                while (results.MoveNextAsync().Result)
                {
                    dataList.AddRange(results.Current);
                }

                return dataList.Count>0;
            }
            catch (System.Exception e)
            {
                Log.Debug($"查询数据库 单表的所以有数据失败  !!! 错误信息：{e}");
                return false;
            }
      
        }
    }

    public static class DBSaveHelper
    {
        public static async ETTask SaveBatch(this DBProxyComponent self, List<ComponentWithId> components)
        {
            Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
            await session.Call(new DBSaveBatchRequest { Components = components });
        }

        public static async ETTask Save(this DBProxyComponent self, ComponentWithId component)
        {
            Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
            await session.Call(new DBSaveRequest { Component = component });
        }
    }

    public static class DBQueryHelper
    {
        public static long GetCount(string collectionName)
        {
             FilterDefinitionBuilder<ComponentWithId> builderFilter = Builders<ComponentWithId>.Filter;

            return  Game.Scene.GetComponent<DBComponent>().GetCollection(collectionName).Find(builderFilter.Empty).CountDocuments();
        }

        public static async ETTask<T> Query<T>(this DBProxyComponent self, long id) where T : ComponentWithId
        {
            Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
            DBQueryResponse dbQueryResponse = (DBQueryResponse)await session.Call(new DBQueryRequest { CollectionName = typeof(T).Name, Id = id });
            return (T)dbQueryResponse.Component;
        }

        /// <summary>
        /// 根据json查询条件查询
        /// </summary>
        /// <param name="self"></param>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async ETTask<List<ComponentWithId>> Query<T>(this DBProxyComponent self, string json) where T : ComponentWithId
        {
            Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.dbAddress);
            DBQueryJsonResponse dbQueryJsonResponse = (DBQueryJsonResponse)await session.Call(new DBQueryJsonRequest { CollectionName = typeof(T).Name, Json = json });
            return dbQueryJsonResponse.Components;
        }
    }
}
 