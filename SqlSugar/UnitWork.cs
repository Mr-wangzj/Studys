using Microsoft.Extensions.Configuration;
using Npgsql;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbType = SqlSugar.DbType;

namespace SqlSugarRepository
{
    public class UnitWork : IUnitWork
    {
        private readonly SqlSugarScope _connection;

        public UnitWork(IConfiguration configuration)
        {

            _connection = new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString =configuration.GetConnectionString("ListingDb"),
                DbType = DbType.PostgreSQL,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true,//开启自动释放模式

            });

        }
        public SqlSugarScope  Connection
        {
            get { return _connection as SqlSugarScope; }
        }
     

        void IUnitWork.BeginTran()
        {
            Connection.BeginTran();
        }

        void IUnitWork.CommitTran()
        {
            Connection.CommitTran();
        }

        void IUnitWork.RollbackTran()
        {
            Connection.RollbackTran();
        }

     
    }
}
