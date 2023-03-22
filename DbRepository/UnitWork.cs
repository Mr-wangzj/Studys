using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    public class UnitWork : IUnitWork, IDisposable
    {
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;
        private bool _disposed = false;

        public UnitWork(IConfiguration configuration)
        {

            _connection= new NpgsqlConnection(configuration.GetConnectionString("ListingDb"));
            
        }
        public IDbConnection Connection
        {
            get { return _connection; }
        }
        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }

        void IUnitWork.BeginTran()
        {
            if (_connection.State!=ConnectionState.Open)
            {
                _connection.Open();
            }
            _transaction =_connection.BeginTransaction();
        }

        void IUnitWork.CommitTran()
        {
            _transaction.Commit();
            Dispose();
        }

        void IUnitWork.RollbackTran()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                return;
            }
            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
            _transaction = null;
            _connection = null;
            _disposed = true;
        }
        ~UnitWork()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
