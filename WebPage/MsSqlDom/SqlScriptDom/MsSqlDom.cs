using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SqlScriptDom
{
    public class MsSqlDom
    {
        /// <summary>
        /// 获得SQL2008的sql片段
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static TSqlScript GetScript(string sql)
        {
            TSqlParser parser1 = new TSql100Parser(false);
            IList<ParseError> errors;
            TSqlFragment result1 = parser1.Parse(new StringReader(sql), out errors);
            TSqlScript script = result1 as TSqlScript;
            return script;
        }
        /// <summary>
        /// 获得sql的第一句如果是select语句对应的语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SelectStatement GetSelectStatement(string sql)
        {
            SelectStatement returnVal = null;
            TSqlScript script = GetScript(sql);
            if (script.Batches != null && script.Batches.Count > 0)
            {
                TSqlBatch batch = script.Batches[0];
                if (batch.Statements != null && batch.Statements.Count > 0)
                {
                    returnVal = script.Batches[0].Statements[0] as SelectStatement;
                }
            }
            return returnVal;
        }
        /// <summary>
        /// 获得sql的第一句如果是select语句对应的语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static QuerySpecification GetSelectQuerySpecification(string sql)
        {
            QuerySpecification returnVal = null;
            SelectStatement selectStatement = GetSelectStatement(sql);
            if (selectStatement != null)
            {
                returnVal = selectStatement.QueryExpression as QuerySpecification;
            }
            return returnVal;
        }
        /// <summary>
        /// 根据sql获得字段和实际字段表达式的对应关系
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetFieldHashtable(string sql)
        {
            Dictionary<string, string> gridFileldHashTable = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(sql))
            {
                QuerySpecification queryExpression = MsSqlDom.GetSelectQuerySpecification(sql);
                if (queryExpression != null)
                {
                    foreach (SelectElement field1 in queryExpression.SelectElements)
                    {
                        SelectScalarExpression field = field1 as SelectScalarExpression;
                        if (field != null)
                        {
                            string key = string.Empty;
                            if (field.ColumnName != null)
                            {
                                key = field.ColumnName.Value;
                            }
                            else
                            {
                                ColumnReferenceExpression express = field.Expression as ColumnReferenceExpression;
                                if (express != null && express.MultiPartIdentifier != null
                                    && express.MultiPartIdentifier.Identifiers != null
                                    && express.MultiPartIdentifier.Identifiers.Count > 0)
                                {
                                    key = express.MultiPartIdentifier.Identifiers[express.MultiPartIdentifier.Identifiers.Count - 1].Value;
                                }
                            }
                            gridFileldHashTable[key] = sql.Substring(field.Expression.StartOffset, field.Expression.FragmentLength);
                        }
                    }
                }
            }
            return gridFileldHashTable;
        }
        #region 获得sql语句中的各个部分
        /// <summary>
        /// 根据queryExpression中从sql获取Field部分
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetFieldsSqlPart(string sql)
        {
            string fieldsSqlPart = string.Empty;
            QuerySpecification queryExpression = MsSqlDom.GetSelectQuerySpecification(sql);
            if (queryExpression != null)
            {
                fieldsSqlPart = MsSqlDom.GetFieldsSqlPart(sql, queryExpression);
            }
            return fieldsSqlPart;
        }
        /// <summary>
        /// 根据queryExpression中从sql获取From部分
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public static string GetFieldsSqlPart(string sql, QuerySpecification queryExpression)
        {
            string FieldsSqlPart = string.Empty;
            if (queryExpression != null && sql != null)
            {
                FieldsSqlPart = sql.Substring(0, queryExpression.FromClause.StartOffset);
            }
            return FieldsSqlPart;
        }
        /// <summary>
        /// 根据queryExpression中从sql获取From部分
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public static string GetFromSqlPart(string sql, QuerySpecification queryExpression)
        {
            string fromSqlPart = string.Empty;
            if (queryExpression != null && queryExpression.FromClause != null)
            {
                fromSqlPart = sql.Substring(queryExpression.FromClause.StartOffset, queryExpression.FromClause.FragmentLength);
            }
            return fromSqlPart;
        }
        /// <summary>
        /// 根据queryExpression中从sql获取From部分
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetFromSqlPart(string sql)
        {
            string fromSqlPart = string.Empty;
            QuerySpecification queryExpression = MsSqlDom.GetSelectQuerySpecification(sql);
            if (queryExpression != null)
            {
                fromSqlPart = MsSqlDom.GetFromSqlPart(sql, queryExpression);
            }
            return fromSqlPart;
        }
        #region 原封不动的获取Order By 部分
        /// <summary>
        /// 根据queryExpression中从sql获取原始的Order by部分
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public static string GetOriginalOrderSqlPart(string sql, QuerySpecification queryExpression)
        {
            string orderSqlPart = string.Empty;
            if (queryExpression.OrderByClause != null && queryExpression.OrderByClause.OrderByElements != null
                && queryExpression.OrderByClause.OrderByElements.Count > 0)
            {
                //sql.Substring((queryExpression.OrderByClause.OrderByElements[0]).StartOffset, (queryExpression.OrderByClause.OrderByElements[0]).FragmentLength)
                int orderElementStartIndex = queryExpression.OrderByClause.OrderByElements[0].StartOffset - queryExpression.OrderByClause.StartOffset;
                orderSqlPart = sql.Substring(queryExpression.OrderByClause.OrderByElements[0].StartOffset, queryExpression.OrderByClause.FragmentLength - orderElementStartIndex);
            }
            return orderSqlPart;
        }
        /// <summary>
        /// 从sql获取原始的Order by部分
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetOriginalOrderSqlPart(string sql)
        {
            string orderSqlPart = string.Empty;
            QuerySpecification queryExpression = MsSqlDom.GetSelectQuerySpecification(sql);
            if (queryExpression != null)
            {
                orderSqlPart = MsSqlDom.GetOriginalOrderSqlPart(sql, queryExpression);
            }
            return orderSqlPart;
        }
        #endregion
        #region 合并同类项（防止一个字段多次Order报错，多次出现的Order By的项取前一项）的获取Order By 部分
        /// <summary>
        /// 合并同类项（防止一个字段多次Order报错）方式，根据queryExpression中从sql获取Order by部分，多次出现的Order By的项取前一项
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public static string GetOrderSqlPart(string sql, QuerySpecification queryExpression)
        {
            string orderSqlPart = string.Empty;
            if (queryExpression.OrderByClause != null && queryExpression.OrderByClause.OrderByElements != null
                && queryExpression.OrderByClause.OrderByElements.Count > 0)
            {
                //sql.Substring((queryExpression.OrderByClause.OrderByElements[0]).StartOffset, (queryExpression.OrderByClause.OrderByElements[0]).FragmentLength)
                List<ExpressionWithSortOrder> orderByElements1 = new List<ExpressionWithSortOrder>(queryExpression.OrderByClause.OrderByElements);
                List<string> orderByList = new List<string>();
                for ( int i=0;i< queryExpression.OrderByClause.OrderByElements.Count;i++)
                {
                    ExpressionWithSortOrder orderItem = queryExpression.OrderByClause.OrderByElements[i];
                    string orderItemSql = sql.Substring(orderItem.StartOffset, orderItem.FragmentLength);
                    if (!orderByList.Contains(orderItemSql))
                    {
                        orderByList.Add(orderItemSql);
                    }
                }
                //int orderElementStartIndex = queryExpression.OrderByClause.OrderByElements[0].StartOffset - queryExpression.OrderByClause.StartOffset;
                orderSqlPart = string.Join(",", orderByList);
                    //sql.Substring(queryExpression.OrderByClause.OrderByElements[0].StartOffset, queryExpression.OrderByClause.FragmentLength - orderElementStartIndex);
            }
            return orderSqlPart;
        }
        /// <summary>
        /// 合并同类项（防止一个字段多次Order报错）方式，从sql获取Order by部分
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetOrderSqlPart(string sql)
        {
            string orderSqlPart = string.Empty;
            QuerySpecification queryExpression = MsSqlDom.GetSelectQuerySpecification(sql);
            if (queryExpression != null)
            {
                orderSqlPart = MsSqlDom.GetOrderSqlPart(sql, queryExpression);
            }
            return orderSqlPart;
        }
        #endregion
        /// <summary>
        /// 根据queryExpression中从sql获取Order by部分，出去的是不带and的
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public static string GetWhereSqlPart(string sql, QuerySpecification queryExpression)
        {
            string whereSql = string.Empty;
            if (queryExpression.WhereClause != null && !string.IsNullOrWhiteSpace(sql))
            {
                #region 这是原来算where部分长度代码，少一位
                //int whereEndOffset = queryExpression.WhereClause.StartOffset;
                //if (queryExpression.OrderByClause != null)
                //{
                //    whereEndOffset = queryExpression.OrderByClause.StartOffset;
                //}
                //else if (queryExpression.HavingClause != null)
                //{
                //    whereEndOffset = queryExpression.HavingClause.StartOffset;
                //}
                //else
                //{
                //    whereEndOffset = sql.Length - 1;
                //}
                // whereEndOffset - queryExpression.WhereClause.StartOffset，
                #endregion
                whereSql = sql.Substring(queryExpression.WhereClause.StartOffset, queryExpression.WhereClause.FragmentLength);
                whereSql = whereSql.TrimStart();
                if (whereSql.StartsWith("where", StringComparison.CurrentCultureIgnoreCase))
                {
                    whereSql = whereSql.Substring(5);
                }
            }
            return whereSql;
        }
        /// <summary>
        /// 根据queryExpression中从sql获取Order by部分
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetWhereSqlPart(string sql)
        {
            string sqlPart = string.Empty;
            QuerySpecification queryExpression = MsSqlDom.GetSelectQuerySpecification(sql);
            if (queryExpression != null)
            {
                sqlPart = MsSqlDom.GetWhereSqlPart(sql, queryExpression);
            }
            return sqlPart;
        }
        #endregion
    }
}
