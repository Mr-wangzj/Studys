using MongoDB.Driver;

namespace MongodbRepository
{
    public interface IMongoDbFactory
    {
        IMongoDatabase Database { get; }
        #region INSERT

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentName"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        Task Insert<T>(string documentName, T document);

        /// <summary>
        /// 新增多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        Task InsertMany<T>(string documentname, IList<T> documents);


        #endregion


        #region SELECT
        /// <summary>
        /// 判断文档存在状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filterexist"></param>
        /// <returns></returns>
        Task<bool> IsExistDocument<T>(string documentname, FilterDefinition<T> filter);

        /// <summary>
        /// 通过条件得到查询的结果个数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<long> GetCount<T>(string documentname, FilterDefinition<T> filter);

        /// <summary>
        /// 通过系统id(ObjectId)获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetDocumentById<T>(string documentname, string id);


        /// <summary>
        /// 通过系统id(ObjectId)获取一个对象同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<T> GetDocumentById<T>(string documentname, string id, ProjectionDefinition<T> fields);


        /// <summary>
        /// 通过指定的条件获取一个对象，如果有多条，只取第一条，同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        T GetDocumentByUserFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields);

        /// <summary>
        /// 获取全部文档
        /// </summary>
        /// <typeparam name="T"></typeparam>       
        /// <param name="documentname"></param>
        /// <returns></returns>
        Task<IList<T>> GetAllDocuments<T>(string documentname);

        /// <summary>
        /// 获取全部文档同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        IList<T> GetAllDocuments<T>(string documentname, ProjectionDefinition<T> fields);

        /// <summary>
        /// 通过一个条件获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="property">字段名</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        Task<IList<T>> GetDocumentsByFilter<T>(string documentname, string property, string value);


        /// <summary>
        /// 通过条件获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IList<T>> GetDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter);

        /// <summary>
        /// 通过条件获取对象,同时过滤字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="property">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        IList<T> GetDocumentsByFilter<T>(string documentname, string property, string value, ProjectionDefinition<T> fields);

        /// <summary>
        /// 通过条件获取对象,同时过滤数据和字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter">过滤器</param>
        /// <param name="fields">要获取的字段</param>
        /// <returns></returns>
        IList<T> GetDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields);

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
        IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, ProjectionDefinition<T> fields, SortDefinition<T> sort, int pageIndex, int pageSize);

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
        IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, SortDefinition<T> sort, int pageIndex, int pageSize);


        /// <summary>
        /// 通过条件获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IList<T> GetPagedDocumentsByFilter<T>(string documentname, FilterDefinition<T> filter, int pageIndex, int pageSize);

        /// <summary>
        /// 获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IList<T> GetPagedDocumentsByFilter<T>(string documentname, SortDefinition<T> sort, int pageIndex, int pageSize);

        /// <summary>
        /// 获取分页的文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IList<T> GetPagedDocumentsByFilter<T>(string documentname, int pageIndex, int pageSize);

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
        Task UpdateReplaceOne<T>(string documentname, string id, T oldinfo);

        /// <summary>
        /// 只能替换一条，如果有多条的话
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <param name="oldinfo"></param>
        Task UpdateReplaceOne<T>(string documentname, FilterDefinition<T> filter, T oldinfo);

        /// <summary>
        /// 更新指定属性值，按ID就只有一条，替换一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <param name="setvalue"></param>
        /// <returns></returns>
        Task Update<T>(string documentname, string id, string property, string value);

        Task Update<T>(string documentname, FilterDefinition<T> filter, UpdateDefinition<T> update);

        Task UpdateMany<T>(string documentname, FilterDefinition<T> filter, UpdateDefinition<T> update);

        #endregion

        #region DELETE
        /// <summary>
        /// 删除一个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete<T>(string documentname, string id);

        Task Delete<T>(string documentname, string property, string value);

        /// <summary>
        /// 通过一个属性名和属性值删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteMany<T>(string documentname, string property, string value);

        /// <summary>
        /// 通过一个属性名和属性值删除多个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentname"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task DeleteMany<T>(string documentname, FilterDefinition<T> filter);

        #endregion
    }
}