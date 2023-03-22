﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    //数据库链接接口
    //打开数据库连接，开关事务

    public   interface IUnitWork
    {
        
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void BeginTran();
        void CommitTran();
        void RollbackTran();    



    }
}
