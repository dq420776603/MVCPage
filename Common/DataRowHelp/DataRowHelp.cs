using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace Common
{
    /// <summary>
    /// 针对 DataRow类的扩展 方法的类
    /// </summary>
    public static class DataRowHelp
    {
        /// <summary>
        /// 将 DataRow对象 转化为 T类型的实例，用泛形 可以 兼顾 强类型检查和 效率（要么，要通过 反射来创建实例）
        /// </summary>
        /// <param name="dr">要转化的DataRow</param>
        /// <param name="type">要转化成什么Type，type要有无参构造函数</param>
        /// <returns></returns>
        public static object ToObject(DataRow dr, Type type)
        {
            object model = type.Assembly.CreateInstance(type.FullName);
            //获得对象 上的所有 相关属性信息
            PropertyInfo[] proInfoList = model.GetType().GetProperties();
            //遍历所有列
            for (int x = 0; x < dr.Table.Columns.Count; x++)
            {
                //将每列的值，对应更新到 对应的属性上面去
                for (int i = 0; i < proInfoList.Length; i++)
                {
                    if (dr.Table.Columns[x].ColumnName.ToUpper().Trim() == proInfoList[i].Name.ToUpper().Trim())
                    {
                        object val = dr[x];
                        if (val is DBNull)
                        {
                            val = null;
                        }
                        object proObj = null;
                        if (val != null)
                        {
                            //如果，是泛型并且是值类型（就是说是空属类型）的话
                            if (proInfoList[i].PropertyType.IsGenericType
                                && proInfoList[i].PropertyType.IsSubclassOf(typeof(System.ValueType)))
                            {
                                Type[] typeList = proInfoList[i].PropertyType.GetGenericArguments();
                                if (typeList != null && typeList.Length > 0)
                                {
                                    proObj = Convert.ChangeType(val, typeList[0]);
                                }
                            }
                            else
                            {
                                proObj = Convert.ChangeType(val, proInfoList[i].PropertyType);
                            }
                        }
                        proInfoList[i].SetValue(model, proObj, new Object[0]);
                    }
                }
            }
            return model;
        }
    }
}