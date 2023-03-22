using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SqlSugar;

namespace SqlSugarRepository
{   //DB基类
    //DB 操作接口实现
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private IUnitWork _unitOfWork;
        //构造函数 注入
        public Repository(IUnitWork unitWork)
        {
            _unitOfWork=    unitWork;
        }
        public Task<int> Add( T Entity)
        {
          return  _unitOfWork.Connection.Insertable(Entity).ExecuteCommandAsync();
        }

        public Task<int> Delete(T Entity)
        {
            return this._unitOfWork.Connection.Deleteable(Entity).ExecuteCommandAsync();
        }

        public List<T>  GetByFITEL(Expression<Func<T,bool>> expression )
        {
            try
            {
                return _unitOfWork.Connection.Queryable<T>().Where(expression).ToList();
            }
            catch (SqlSugarException ex )
            {

                throw;
            }
          
        }

        public T GetT(Expression<Func<T, bool>> expression)
        {
            return _unitOfWork.Connection.Queryable<T>().First(expression);
        }
        public Task<int> Update(T Entity)
        {
            return _unitOfWork.Connection.Updateable(Entity).ExecuteCommandAsync();
        }

       
       

       
    }
}
