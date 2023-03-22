using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarRepository
{
    //数据库链接接口
    //打开数据库连接，开关事务

    public   interface IUnitWork
    {

        SqlSugarScope Connection { get; }
        void BeginTran();
        void CommitTran();
        void RollbackTran();    



    }
}
