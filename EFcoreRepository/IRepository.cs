using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFcoreRepository
{
    //创建DB操作接口
    public interface IRepository<T> where T : class
    {
        Task<T >Find(int ID);
        Task<int> Add(T Entity);
        Task<int> Update(T Entity);
        Task<int> Delete(T Entity);
        Task<List<T>> SelectAsync(Expression<Func<T, bool>> where);

        void begin();
        void commit();
        void rollback();
    }
}
