using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Npgsql;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace DbRepository
{   //DB基类
    //DB 操作接口实现
    public class Repository<T> : IRepository<T> where T : class
    {
        private IUnitWork _unitOfWork;
        //构造函数 注入
        public Repository(IUnitWork unitWork)
        {
            _unitOfWork=    unitWork;
        }
        public Task<int> Add( T Entity)
        {
            return _unitOfWork.Connection.InsertAsync(Entity);
        }

        public Task<bool> Delete(T Entity)
        {
            return this._unitOfWork.Connection.DeleteAsync(Entity);
        }

        public Task<IEnumerable<T>> GetBySql(string sql, Dictionary<string, object> parameter)
        {
            return _unitOfWork.Connection.QueryAsync<T>(sql, parameter);
        }

        public Task<bool> Update(T Entity)
        {
            return _unitOfWork.Connection.UpdateAsync(Entity);
        }

       
        public Task<int> Execute(string sql)
        {
            return this._unitOfWork.Connection.ExecuteAsync(sql);
        }

        public DataTable GetDataTableBySql(string sql)
        {
            DataTable table = new DataTable();
            var d = this._unitOfWork.Connection.ExecuteReader(sql);
            table.Load(d);
            return table;
        }

        public IEnumerable<T> GetALL()
        {
            return _unitOfWork.Connection.GetAll<T>();
        }

        public  T  GetDataByID(int id)
        {
            return _unitOfWork.Connection.Get<T>(id);
        }

       
    }
}
