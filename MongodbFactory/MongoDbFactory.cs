using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongodbRepository
{
    public class MongoDbFactory:IMongoDbFactory
    {
        private string databaseName = string.Empty;
        private readonly IMongoClient _client; // 客户端
        private  IMongoDatabase database=null ; //表
        /// <summary>
        /// 创建Mongdb连接客户端
        /// </summary>
        /// <param name=""></param>
        public MongoDbFactory()
		{
            _client = new MongoClient("mongodb://127.0.0.1:27017");
            database=_client.GetDatabase("School");
        }

        /// <summary>
        /// 数据库
        /// </summary>
        public string DatabaseName
        {
            get { return databaseName; }
            set
            {
                databaseName = value;
                database = _client.GetDatabase(databaseName);
            }
        }

        public IMongoDatabase Database => database;

        /// <summary>
        /// 获取数据库表
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName">数据库</param>
        public MongoDbFactory(string databaseName)
        {
            _client = new MongoClient("mongodb://127.0.0.1:27017");
            database = _client.GetDatabase(databaseName);
        }
        #region 配置信息


        /// <summary>
        /// 执行命令，命令请参考MongoCommand,命令太多，不一一展示，传入的就是里面的字符串，有些命令执行需要连接到admin表
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public BsonDocument RunCommand(string cmdText)
        {
            return database.RunCommand<BsonDocument>(cmdText);
        }

        public IList<BsonDocument> GetDatabase()
        {
            return _client.ListDatabases().ToList();
        }

        #endregion


        #region INSERT

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="documentName">数据库表</param>
        /// <param name="document"></param>
        /// <returns></returns>
        public Task Insert<T>(string documentName, T document)
        {
            return database.GetCollection<T>(documentName).InsertOneAsync(document);
        }

        /// <summary>
        /// 新增多个文档
        /// </summary>
        /// <param name="documentname"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task InsertMany<T>(string documentname, IList<T> documents)
        {
            return database.GetCollection<T>(documentname).InsertManyAsync(documents);
        }


        #endregion


        #region SELECT

        /// <summary>
        /// 判断文档存在状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filterexist"></param>
        /// <returns></returns>
        public async Task<bool> IsExistDocument<T>(string documentname, FilterDefinition<T> filter)
        {
            return (await database.GetCollection<T>(documentname).CountAsync(filter)) > 0;
        }

        /// <summary>
        /// 通过条件得到查询的结果个数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<long> GetCount<T>(string documentname, FilterDefinition<T> filter)
        {
            return database.GetCollection<T>(documentname).CountAsync(filter);
        }

        /// <summary>
        /// 通过系统id(ObjectId)获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetDocumentById<T>(string documentname, string id)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            var result = database.GetCollection<T>(documentname).FindAsync(filter);
            return (await result).FirstOrDefault();
        }

        /// <summary>
        /// 通过系统id(ObjectId)获取一个对象同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task<T> GetDocumentById<T>(string documentname, string id, ProjectionDefinition<T> fields)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            return   database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).FirstOrDefault();
        }


        /// <summary>
        /// 通过指定的条件获取一个对象，如果有多条，只取第一条，同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public T GetDocumentByUserFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields)
        {
            return database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).FirstOrDefault();
        }


        /// <summary>
        /// 获取全部文档
        /// </summary>
        /// <typeparam name="T"></typeparam>       
        /// <param name="documentname"></param>
        /// <returns></returns>
        public async Task<IList<T>> GetAllDocuments<T>(string documentname)
        {
            var filter = Builders<T>.Filter.Empty;
            return (await database.GetCollection<T>(documentname).FindAsync(filter)).ToList();
        }

        /// <summary>
        /// 获取全部文档同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        public IList<T> GetAllDocuments<T>(string documentname, ProjectionDefinition<T> fields)
        {
            var filter = Builders<T>.Filter.Empty;
            return database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).ToList();
        }

        /// <summary>
        /// 通过一个条件获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="property">字段名</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public async Task<IList<T>> GetDocumentsByFilter<T>(string documentname, string property, string value)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            return (await database.GetCollection<T>(documentname).FindAsync(filter)).ToList();
        }


        /// <summary>
        /// 通过条件获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IList<T>> GetDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter)
        {
            return (await database.GetCollection<T>(documentname).FindAsync(filter)).ToList();
        }

        /// <summary>
        /// 通过条件获取对象,同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="property">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, string property, string value, ProjectionDefinition<T> fields)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            return database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).ToList();
        }


        /// <summary>
        /// 通过条件获取对象,同时过滤数据和字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter">过滤器</param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        public IList<T> GetDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields)
        {
            return database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).ToList();
        }

        /// <summary>
        /// 通过条件获取分页的文档并排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields, SortDefinition<T> sort, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).Sort(sort).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).Project<T>(fields).Sort(sort).ToList();
            }
            return result;
        }


        /// <summary>
        /// 通过条件获取分页的文档并排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, SortDefinition<T> sort, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Sort(sort).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).Sort(sort).ToList();
            }
            return result;
        }

        /// <summary>
        /// 通过条件获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, SortDefinition<T> sort, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            var filter = Builders<T>.Filter.Empty;
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Sort(sort).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).Sort(sort).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPagedDocumentsByFilter<T>(string documentname, int pageIndex, int pageSize)
        {
            IList<T> result = new List<T>();
            var filter = Builders<T>.Filter.Empty;
            if (pageIndex != 0 && pageSize != 0)
            {
                result = database.GetCollection<T>(documentname).Find(filter).Skip(pageSize * (pageIndex - 1)).Limit(pageSize).ToList();
            }
            else
            {
                result = database.GetCollection<T>(documentname).Find(filter).ToList();
            }
            return result;
        }


        #endregion


        #region UPDATE

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filterexist"></param>
        /// <param name="id"></param>
        /// <param name="oldinfo"></param>
        /// <returns></returns>
        public Task UpdateReplaceOne<T>(string documentname, string id, T oldinfo)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            return database.GetCollection<T>(documentname).ReplaceOneAsync(filter, oldinfo);
        }

        /// <summary>
        /// 只能替换一条，如果有多条的话
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="oldinfo"></param>
        public Task UpdateReplaceOne<T>(string documentname, FilterDefinition<T> filter, T oldinfo)
        {
            return database.GetCollection<T>(documentname).ReplaceOneAsync(filter, oldinfo);
        }

        /// <summary>
        /// 更新指定属性值，按ID就只有一条，替换一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <param name="setvalue"></param>
        /// <returns></returns>
        public Task Update<T>(string documentname, string id, string property, string value)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filter = Builders<T>.Filter.Eq("_id", oid);
            var update = Builders<T>.Update.Set(property, value);
            return database.GetCollection<T>(documentname).UpdateOneAsync(filter, update);
        }

        public Task Update<T>(string documentname, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return database.GetCollection<T>(documentname).UpdateOneAsync(filter, update);
        }

        public Task UpdateMany<T>(string documentname, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return database.GetCollection<T>(documentname).UpdateManyAsync(filter, update);
        }

        #endregion

        #region DELETE

        /// <summary>
        /// 删除一个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task Delete<T>(string documentname, string id)
        {
            ObjectId oid = ObjectId.Parse(id);
            var filterid = Builders<T>.Filter.Eq("_id", oid);
            return database.GetCollection<T>(documentname).DeleteOneAsync(filterid);
        }

        public Task Delete<T>(string documentname, string property, string value)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            return database.GetCollection<T>(documentname).DeleteOneAsync(filter);
        }

        /// <summary>
        /// 通过一个属性名和属性值删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteMany<T>(string documentname, string property, string value)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(property, value);
            return database.GetCollection<T>(documentname).DeleteManyAsync(filter);
        }

        /// <summary>
        /// 通过一个属性名和属性值删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task DeleteMany<T>(string documentname, FilterDefinition<T> filter)
        {
            return database.GetCollection<T>(documentname).DeleteManyAsync(filter);
        }

        #endregion
    }
}
