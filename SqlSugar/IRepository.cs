using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarRepository
{
    //创建DB操作接口
    public interface IRepository<T> where T : class
    {
      
        Task<int> Add(T Entity);
        Task<int> Update(T Entity);
        Task<int> Delete(T Entity);
        List<T> GetByFITEL(Expression<Func<T, bool>> expression);
        T GetT(Expression<Func<T, bool>> expression);
    }
}
