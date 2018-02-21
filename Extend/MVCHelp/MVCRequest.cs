using DBHelp.SqlScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extend.MVCHelp
{
    public class MVCRequest : Request
    {
        private string _sort;
        private string _order;

        public string sort { set { _sort = value; } get { return _sort; } }
        public string order { set { _order = value; } get { return _order; } }
        public List<string> List_sort
        {
            get
            {
                return _sort == null ? new List<string>() : _sort.Trim(',').Split(',').ToList();
            }
        }
        public List<string> List_order
        {
            get
            {
                return _sort == null ? new List<string>() : _order.Trim(',').Split(',').ToList();
            }
        }
        #region 对于原生sql不实用
        /// <summary>
        /// 多个排序表达式集合
        /// </summary>
        public List<string> ListOrderExpression
        {
            get
            {
                List<string> listOrderexpression = new List<string>();
                List<string> listSort = List_sort;
                List<string> listOrder = List_order;
                for (int itemIndex = 0; itemIndex < listSort.Count; itemIndex++)
                {
                    listOrderexpression.Add($"[{listSort[itemIndex]}] {listOrder[itemIndex]}");
                }
                return listOrderexpression;
            }
        }
        /// <summary>
        /// 多个排序表达式字符串
        /// </summary>
        public string ListOrderExpressionString
        {
            get
            {
                return string.Join(",", ListOrderExpression);
            }
        }
        #endregion
        #region 考虑sql字段别名的情况
        /// <summary>
        /// 根据sql多个排序表达式集合
        /// </summary>
        public List<string> GetSqlOrderExpressionList(string sql)
        {
            List<string> listOrderexpression = new List<string>();
            Dictionary<string, string> hashField = MsSqlDom.GetFieldHashtable(sql);
            List<string> listSort = List_sort;
            List<string> listOrder = List_order;
            for (int itemIndex = 0; itemIndex < listSort.Count; itemIndex++)
            {
                string initialField = listSort[itemIndex];
                if (hashField.Keys.Contains(initialField))
                {
                    initialField = hashField[initialField];
                }
                listOrderexpression.Add($"{initialField} {listOrder[itemIndex]}");
            }
            return listOrderexpression;
        }
        /// <summary>
        /// 根据sql多个排序表达式
        /// </summary>
        public string GetSqlOrderExpression(string sql)
        {
            return string.Join(",", GetSqlOrderExpressionList(sql));
        }
        #endregion
    }
    /// <summary>
    /// 用于BLL方法提传入条件
    /// </summary>
    public class Request
    {
        #region bootstrapTable用
        /// <summary>
        /// 要读取的第一行
        /// </summary>
        protected int offset = -1;
        /// <summary>
        /// 要读取的第一行，NHibernate读取的是这个属性
        /// </summary>
        public int Offset
        {
            get
            {
                if (offset >= 0)
                {
                    return offset;
                }
                else
                {
                    return MVCRequestHelp.ComputeOffset(Page, Rows);
                }
            }
            set
            {
                offset = value;
            }
        }
        /// <summary>
        /// 读取多少行，如果是bootstrapTable，会接收这个属性
        /// </summary>
        public int Limit
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
            }
        }
        #endregion
        #region EasyUI.datagrid用
        /// <summary>
        /// 读取多少行
        /// </summary>
        protected int rows = -1;
        /// <summary>
        /// 读取多少行
        /// </summary>
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
            }
        }
        /// <summary>
        /// 第几页，EasyUI.datagrid用
        /// </summary>
        protected int page = -1;
        /// <summary>
        /// 第几页，EasyUI.datagrid用
        /// </summary>
        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                page = value;
            }
        }
        #endregion
    }
}
