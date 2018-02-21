using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Common
{
    public static partial class DataTableEx
    {
        /// <summary>
        /// 将DataTable中某一列的所有 值转化为 List<object> 返回
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static List<object> GetObjectList(DataTable dataSource, string field)
        {
            List<object> objList = new List<object>();
            foreach (DataRow dr in dataSource.Rows)
            {
                objList.Add(dr[field]);
            }
            return objList;
        }
        /// <summary>
        /// 找到 DataTable 中 （如果是树型数据的话），递归寻找 对应idField=idValue的所有子节点
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="idField"></param>
        /// <param name="idValue"></param>
        /// <param name="paterField"></param>
        /// <returns></returns>
        public static DataTable GetSubTable(DataTable dataSource, string idField, object idValue, string paterField)
        {
            List<DataRow> rowList = GetSubRowList(dataSource, idField, idValue, paterField);
            DataTable dt = dataSource.Clone();
            foreach (DataRow dr in rowList)
            {
                dt.ImportRow(dr);
            }
            return dt;
        }
        /// <summary>
        /// 找到 DataTable 中 （如果是树型数据的话），递归寻找 对应idField=idValue的所有子节点
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="idField"></param>
        /// <param name="idValue"></param>
        /// <returns></returns>
        public static List<DataRow> GetSubRowList(DataTable dataSource, string idField, object idValue, string paterField)
        {
            //先 复制一个 dataSource的副本，因为，下文 要 改 dataSource的一些属性
            DataTable dt = dataSource.Copy();
            //将 副本的 筛选器 设置为 idField=idValue 
            dt.DefaultView.RowFilter = string.Format("{0}={1}", paterField, idValue);
            //返回 递归得到的值
            return GetSubRowListRecursion(dt.DefaultView, idField, paterField);
        }
        /// <summary>
        /// 递归寻找 对应idField=idValue的所有子节点
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="idField"></param>
        /// <param name="paterField"></param>
        /// <returns></returns>
        public static List<DataRow> GetSubRowListRecursion(DataView dv, string idField, string paterField)
        {
            List<DataRow> rowList = new List<DataRow>();
            for (int i = 0; i < dv.Count; i++)
            {
                DataRowView dr = dv[i];
                string oldFilter = dv.RowFilter;
                rowList.Add(dr.Row);
                dv.RowFilter = string.Format("{0}={1}", paterField, dr[idField]);
                rowList.AddRange(GetSubRowListRecursion(dv, idField, paterField));
                dv.RowFilter = oldFilter;
            }
            return rowList;
        }
        /// <summary>
        /// 找到 DataTable 中 （如果是树型数据的话），找到 对应的根节点
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="idField"></param>
        /// <param name="paterField"></param>
        /// <returns></returns>
        public static DataRow GetRootRow(DataTable dataSource, string idField, string paterField)
        {
            return GetRootRow(dataSource, idField, paterField, "{0}");
        }
        /// <summary>
        /// 找到 DataTable 中 （如果是树型数据的话），找到 对应的根节点
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="idField"></param>
        /// <param name="paterField"></param>
        /// <param name="paterFieldFormat">如果是字符型，则为'{0}'</param>
        /// <returns></returns>
        public static DataRow GetRootRow(DataTable dataSource, string idField, string paterField, string paterFieldFormat)
        {
            if (string.Format("{0}", paterFieldFormat).Trim() == string.Empty)
            {
                paterFieldFormat = "{0}";
            }
            DataRow dr = null;
            if (dataSource != null && dataSource.Rows.Count > 0)
            {
                dr = dataSource.Rows[0];
                DataView dv = dataSource.DefaultView;
                //先将 旧 RowFilter记录 一下
                string oldFilter = dv.RowFilter;
                //通过 设置  RowFilter 直到  dv.Count<=0时，就是到根节点了
                DataRowView drv = null;
                while (dv.Count > 0)
                {
                    drv = dv[0];
                    //以当前 节点的父节点为 id的节点
                    string paterFilter = string.Format(paterFieldFormat, drv[paterField]);
                    //因为，为 NULL 的话，会报错的
                    if (paterFilter == string.Empty)
                    {
                        paterFilter = "null";
                    }
                    dv.RowFilter = string.Format(@" {0}={1} ", idField, paterFilter);
                }
                if (drv != null)
                {
                    dr = drv.Row;
                }
                //设置回原来的筛选器
                dv.RowFilter = oldFilter;
            }
            return dr;
        }
        /// <summary>
        /// 找到 DataTable 中 （如果是树型数据的话），找到 对应的根节点所对应的ID值
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="idField"></param>
        /// <param name="paterField"></param>
        /// <returns></returns>
        public static object GetRootID(DataTable dataSource, string idField, string paterField)
        {
            object rootId = null;
            DataRow dr = GetRootRow(dataSource, idField, paterField);
            if (dr != null)
            {
                rootId = dr[idField];
            }
            return rootId;
        }
        /// <summary>
        /// 将 任意数据 源 转化为 DataTable
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(object dataSource)
        {
            //对DataTable类型特殊处理一下,因为,Grid碰上DataTable数据源的时候,实际是绑定的DataView
            if (dataSource is DataTable)
            {
                dataSource = (dataSource as DataTable).Copy();
                return dataSource as DataTable;
            }
            DataTable dt = new DataTable();
            //因为，数据源不确定是什么类型，所以，先用.net本身的控件 先绑定一遍，这样，到了下文可以，从drList上来获取数据，逻辑，就比较确定了
            DataGrid rowList = new DataGrid();
            rowList.AutoGenerateColumns = true;
            // 为了设置 让每列 都能 不受 DataGrid自己加的 Html的影响，而响应此事件
            rowList.ItemDataBound += delegate(object sender, DataGridItemEventArgs e)
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    for (int x = 0; x < e.Item.Cells.Count; x++)
                    {
                        dt.Columns.Add(e.Item.Cells[x].Text);
                    }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRow newDr = dt.NewRow();
                    for (int x = 0; x < e.Item.Cells.Count; x++)
                    {
                        newDr[x] = DataBinder.Eval(e.Item.DataItem, dt.Columns[x].ColumnName);
                    }
                    dt.Rows.Add(newDr);
                }
            };
            rowList.DataSource = dataSource;
            rowList.DataBind();
            return dt;
        }
    }
}
