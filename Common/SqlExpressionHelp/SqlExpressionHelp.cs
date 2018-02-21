using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class SqlExpressionHelp
    {
        /// <summary>
        /// 如果为空或string.Empty，则返回string.Empty
        /// </summary>
        /// <param name="paramerVale"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public static string GetExpression(object paramerVale, string whereExpression)
        {
            string returnExpression = string.Empty;
            if (string.Format("{0}", paramerVale).Trim() != string.Empty)
            {
                returnExpression = whereExpression;
            }
            return returnExpression;
        }
        /// <summary>
        /// 如果转换不成时间类型，则返回string.Empty
        /// </summary>
        /// <param name="paramerVale"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public static string GetDateExpression(object paramerVale, string whereExpression)
        {
            string returnExpression = string.Empty;
            DateTime paramerValeDate = DateTime.Now;
            if (DateTime.TryParse(string.Format("{0}", paramerVale).Trim(), out paramerValeDate))
            {
                returnExpression = whereExpression;
            }
            return returnExpression;
        }
        /// <summary>
        /// 如果转换不成Decimal，则返回string.Empty
        /// </summary>
        /// <param name="paramerVale"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public static string GetDecimalExpression(object paramerVale, string whereExpression)
        {
            string returnExpression = string.Empty;
            decimal paramerValeDate;
            if (decimal.TryParse(string.Format("{0}", paramerVale).Trim(), out paramerValeDate))
            {
                returnExpression = whereExpression;
            }
            return returnExpression;
        }
    }
}
