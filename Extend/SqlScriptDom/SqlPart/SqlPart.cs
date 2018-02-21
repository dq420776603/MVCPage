using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extend.SqlScriptDom.SqlPartN
{
    /// <summary>
    /// 代表sql语句的各个部分
    /// </summary>
    public class SqlPart
    {
        public string FieldsSqlPart
        {
            get;
            set;
        }
        public string FromSqlPart
        {
            get;
            set;
        }
        public string WhereSqlPart
        {
            get;
            set;
        }
        public string OrderSqlPart
        {
            get;
            set;
        }
    }
}
