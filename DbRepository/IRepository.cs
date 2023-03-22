using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    //创建DB操作接口
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetALL();
        Task<int> Execute(string sql);
        Task<int> Add(T Entity);
        Task<bool> Update(T Entity);
        Task<bool> Delete(T Entity);
        T GetDataByID(int id);
        Task<IEnumerable<T>>GetBySql(string sql, Dictionary<string, object> parameter);
        DataTable GetDataTableBySql(string sql);
    }
}
