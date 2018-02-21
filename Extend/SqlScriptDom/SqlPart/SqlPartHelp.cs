using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extend.SqlScriptDom.SqlPartN
{
    /// <summary>
    /// 代表sql语句的各个部分
    /// </summary>
    public class SqlPartHelp
    {
        /// <summary>
        /// 转化成sqlPart对应的Count语句
        /// </summary>
        /// <param name="sqlPart"></param>
        /// <param name="countFieldName"></param>
        /// <returns></returns>
        public static string GetCountSql(SqlPart sqlPart,string countFieldName= "total")
        {
            string sql = string.Empty;
            if (sqlPart != null)
            {
                #region where部分
                string whereSqlPart = GetWhereSqlPart(sqlPart.WhereSqlPart);
                #endregion
                sql = $@"select count(1) as {countFieldName}
                            from (select 1 as rowTotal
                                   {sqlPart.FromSqlPart}
                                   {whereSqlPart}) t ";
            }
            return sql;
        }
        /// <summary>
        /// 将sql+request表达的部分，重新构建sql
        /// </summary>
        /// <param name="sqlPart"></param>
        /// <returns></returns>
        public static string ConvertToSqlString(SqlPart sqlPart)
        {
            string returnSql = string.Empty;
            if (sqlPart != null)
            {
                #region where部分
                string whereSqlPart = GetWhereSqlPart(sqlPart.WhereSqlPart);
                #endregion
                #region 排序部分
                string orderSqlPart = GetOrderBySqlPart(sqlPart.OrderSqlPart);
                #endregion

                returnSql = sqlPart.FieldsSqlPart + " "
                             + sqlPart.FromSqlPart + " "
                             + whereSqlPart + " "
                             + orderSqlPart;
            }
            return returnSql;
        }
        /// <summary>
        /// 获取+ order by 关键词的整个order by 部分
        /// </summary>
        /// <param name="whereSqlPart"></param>
        /// <returns></returns>
        public static string GetOrderBySqlPart(string orderSqlPart)
        {
            //如果经过上文，有order by 了，则要加 order by 关键词
            if (!string.IsNullOrWhiteSpace(orderSqlPart))
            {
                orderSqlPart = " order by " + orderSqlPart;
            }
            return orderSqlPart;
        }
        /// <summary>
        /// 获取+ where 关键词的整个where 部分
        /// </summary>
        /// <param name="whereSqlPart"></param>
        /// <returns></returns>
        public static string GetWhereSqlPart(string whereSqlPart)
        {
            if (!string.IsNullOrWhiteSpace(whereSqlPart))
            {
                whereSqlPart = " where " + whereSqlPart;
            }
            return whereSqlPart;
        }
    }
}
