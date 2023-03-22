using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace EFcoreRepository
{   //DB基类
    //DB 操作接口实现
    public class Repository<T> : IRepository<T> where T : class
    {
        private Dbfactory _unitOfWork;
        protected readonly DbSet<T> dbSet;
        IDbContextTransaction tran = null;
        //构造函数 注入
        public Repository()
        {
            _unitOfWork=    new Dbfactory();
            dbSet = _unitOfWork.Set<T>();
        }
        public async Task<int> Add(T Entity)
        {

            _unitOfWork.Add(Entity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> Delete(T Entity)
        {
            this._unitOfWork.Remove(Entity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<T>> SelectAsync(Expression<Func<T, bool>> where)
        {
            var entitys = await dbSet.Where(where).ToListAsync();
            return entitys;
        }


        public async Task<int> Update(T Entity)
        {
            try
            {
                _unitOfWork.Update(Entity);
                return await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

                throw;
            }
           
        }

        public void begin()
        {
            if (_unitOfWork.Database.CurrentTransaction==null)
            {
                tran=  _unitOfWork.Database.BeginTransaction();
            }
        }
        public void commit()
        {
            if (_unitOfWork.Database.CurrentTransaction!=null)
            {
                tran.Commit();
                tran.Dispose();
            }
            else
            {
                tran=_unitOfWork.Database.CurrentTransaction;
                tran.Dispose();
            }

        }
        public void rollback()
        {
            if (_unitOfWork.Database.CurrentTransaction!=null)
            {
                tran.Rollback();
                tran.Dispose();
            }
            else
            {
                tran=_unitOfWork.Database.CurrentTransaction;
            }
        }

        public async Task<T> Find(int ID)
        {
            return await dbSet.FindAsync(ID);
        }
    }
}
