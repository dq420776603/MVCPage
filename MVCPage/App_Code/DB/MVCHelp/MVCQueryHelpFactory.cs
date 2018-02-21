using NHibernateHelp.MVCHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DB.MVCHelp
{
    public class MVCQueryHelpFactory
    {
        /// <summary>
        /// 优先从web.config中取Ht属性对应的连接字符串
        /// </summary>
        /// <returns></returns>
        public static MVCQueryHelp Create()
        {
            MVCQueryHelp help = new MVCQueryHelp();
            help.Ht = SpringDBManager.Instance.GetHibernateTemplate();
            return help;
        }
        ///// <summary>
        ///// 取BaseDataConnection.Effect='main'字段来获取Ht属性对应的连接字符串
        ///// </summary>
        ///// <returns></returns>
        //public static MVCQueryHelp CreateMain()
        //{
        //    return CreateByEffect("main");
        //}
        ///// <summary>
        ///// 取BaseDataConnection.Effect='read'字段来获取Ht属性对应的连接字符串
        ///// </summary>
        ///// <returns></returns>
        //public static MVCQueryHelp CreateRead()
        //{
        //    return CreateByEffect("read");
        //}
        ///// <summary>
        ///// 取BaseDataConnection.Effect='effect'字段来获取Ht属性对应的连接字符串
        ///// </summary>
        ///// <param name="effect"></param>
        ///// <returns></returns>
        //public static MVCQueryHelp CreateByEffect(string effect)
        //{
        //    MVCQueryHelp help = new MVCQueryHelp();
        //    help.Ht = SpringDBManager.Instance.GetHibernateTemplate(effect);
        //    return help;
        //}
        /// <summary>
        /// 用connectionString来当Ht的ConnectionString属性
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static MVCQueryHelp CreateByConnectionString(string connectionString)
        {
            MVCQueryHelp help = new MVCQueryHelp();
            help.Ht = SpringDBManager.Instance.GetHibernateTemplateByConnectionString(connectionString);
            return help;
        }
    }
}
