using SqlScriptDom;
using SqlScriptDom.SqlPartN;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebPageHelp.MVCHelp
{
    public class MVCRequestHelp
    {
        #region 帮助转换排序部分
        /// <summary>
        /// 根据request获得
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetOrderSqlPart(MVCRequest request, string sql)
        {
            string orderPartStr = string.Empty;
            if (request != null)
            {
                orderPartStr = request.GetSqlOrderExpression(sql);
            }
            return orderPartStr;
        }
        #endregion

        /// <summary>
        /// 将sql+request表达的部分，重新构建sql，不考虑NHibernate参数为:的情况
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlPart RefactorToSqlPart(string sql)
        {
            string fieldsSqlPart = MsSqlDom.GetFieldsSqlPart(sql);
            string fromSqlPart = MsSqlDom.GetFromSqlPart(sql);
            #region where部分
            string whereSqlPart = MsSqlDom.GetWhereSqlPart(sql);
            #endregion
            #region 排序部分
            string orderSqlPart = MsSqlDom.GetOrderSqlPart(sql);

            SqlPart sqlPart = new SqlPart()
            {
                FieldsSqlPart = fieldsSqlPart, //参数符号替回来，如果传进来的是 @，正好替成:
                FromSqlPart = fromSqlPart,
                WhereSqlPart = whereSqlPart,
                OrderSqlPart = orderSqlPart
            };
            return sqlPart;
        }
        /// <summary>
        /// 将sql+request表达的部分，重新构建sql，不考虑NHibernate参数为:的情况
        /// </summary>
        /// <param name="request"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlPart RefactorToSqlPart(MVCRequest request, string sql)
        {
            #region 以后删掉
            //string fieldsSqlPart = MsSqlDom.GetFieldsSqlPart(sql);
            //string fromSqlPart = MsSqlDom.GetFromSqlPart(sql);
            //#region where部分
            //string whereSqlPart = MsSqlDom.GetWhereSqlPart(sql);
            //#endregion
            //#region 排序部分
            //string orderSqlPart = MsSqlDom.GetOrderSqlPart(sql);
            #endregion
            SqlPart sqlPart1 = RefactorToSqlPart(sql);
            string orderSqlPart = sqlPart1.OrderSqlPart;
            string joinOrderStr = string.Empty;
            //如果语句中原来有order by 部分，则和EasyUIRequest中表示的order by部分的连接符为 ","
            if (!string.IsNullOrWhiteSpace(orderSqlPart))
            {
                joinOrderStr = ",";
            }
            string easyUIRequestOrder = MVCRequestHelp.GetOrderSqlPart(request, sql);
            //easyUIRequestOrder什么都没有，+后会在原来有order by 部分时会有错
            if (!string.IsNullOrWhiteSpace(easyUIRequestOrder))
            {
                orderSqlPart = easyUIRequestOrder + joinOrderStr + orderSqlPart;
            }
            #endregion
            SqlPart sqlPart = new SqlPart()
            {
                FieldsSqlPart = sqlPart1.FieldsSqlPart, //参数符号替回来，如果传进来的是 @，正好替成:
                FromSqlPart = sqlPart1.FromSqlPart,
                WhereSqlPart = sqlPart1.WhereSqlPart,
                OrderSqlPart = orderSqlPart
            };
            return sqlPart;
        }
        /// <summary>
        /// 根据page页和rows行算起始行
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static int ComputeOffset(int page, int rows)
        {
            return Math.Max((page - 1), 0) * rows;
        }
    }
}
