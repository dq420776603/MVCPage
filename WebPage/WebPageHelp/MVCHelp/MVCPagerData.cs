using System;
using System.Collections.Generic;
using System.Text;

namespace WebPageHelp.MVCHelp
{
    /// <summary>
    /// EasyUI的Grid用的返回的分页数据结构
    /// </summary>
    public class MVCPagerData
    {
        /// <summary>
        /// 当页数据
        /// </summary>
        public virtual object rows
        {
            get;
            set;
        }
        /// <summary>
        /// 所有页总共数据行数
        /// </summary>
        public virtual int total
        {
            get;
            set;
        }
    }
}
