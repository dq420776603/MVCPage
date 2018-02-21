using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Common;
using DAL.DB.MVCHelp;
using Extend.MVCHelp;
using NHibernateHelp.MVCHelp;
using System.Web.Script.Serialization;

namespace MVCPage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetSql(IDictionary<string, object> args)
        {
            string sql = $@"SELECT kehu.Name AS kehu,
                                    ca.Shfzh,
                                    ca.Account,
                                    ca.Cardno,
                                    ca.Xm,
                                    ca.Gqsd,
                                    ca.Khrq,
                                    ipici.Jdsj AS Jdsj,
                                    ca.Dqsj,
                                    us.Code AS ywy,
                                    sc.Name AS State,
                                    ipici.Code AS Jdpc,
                                    pici.Name AS Shouci,
                                    ca.Shengfen,
                                    ca.Chengshi,
                                    bat.FullName AS fenzhi,
                                    ca.Huobi,
                                    ca.Qkje,
                                    ca.Zxqke,
                                    ca.Zxqkerq,
                                    groupdzd.shje AS shje,
                                    ca.Id,
                                    ca.XybaseId
                            FROM dbo.CardInfo ca WITH(NOLOCK)
                            LEFT JOIN dbo.Xykehu kehu WITH(NOLOCK)
                            ON kehu.Id = ca.XyKehuId
                            LEFT JOIN dbo.InPici ipici WITH(NOLOCK)
                            ON ipici.Id = ca.InPiciId
                            LEFT JOIN dbo.VCardinfo vc WITH(NOLOCK)
                            ON ca.Id = vc.Id
                            LEFT JOIN dbo.Users us WITH(NOLOCK)
                            ON (us.id = vc.RelationId
                            AND vc.BaseRelationId = 'Users')
                            LEFT JOIN dbo.BaseSysCode sc WITH(NOLOCK)
                            ON sc.id = vc.State 
                            LEFT JOIN dbo.Pici pici WITH(NOLOCK)
                            ON pici.Id = ca.PiciId
                            LEFT JOIN BaseOrganiseTree bat WITH(NOLOCK)
                            ON bat.Id = vc.RelationId
                            LEFT JOIN(SELECT dzd.CardInfoId,
                                             SUM(dzd.Hkmx) AS shje
                                      FROM dbo.Xydzd dzd WITH(NOLOCK)
                                      WHERE ISNULL(dzd.IsDel,0)=0
                                      GROUP BY dzd.CardInfoId)groupdzd
                            ON groupdzd.CardInfoId = ca.Id
                            WHERE ISNULL(ca.IsDel,0)=0
                            {
                                SqlExpressionHelp.GetExpression(Convert.ToString(args["Xm"]), " AND ca.Xm LIKE isnull(@Xm,'')+'%' ")
                            }
                            {
                                SqlExpressionHelp.GetExpression(Convert.ToString(args["Shfzh"]), " AND ca.Shfzh LIKE isnull(@Shfzh,'')+'%' ")
                            }
                            {
                                SqlExpressionHelp.GetExpression(Convert.ToString(args["Account"]), " AND ca.Account LIKE isnull(@Account,'')+'%' ")
                            }
                            {
                               SqlExpressionHelp.GetExpression(Convert.ToString(args["Cardno"]), " AND ca.Cardno LIKE isnull(@Cardno,'')+'%' ")
                            }
                            {
                               SqlExpressionHelp.GetDecimalExpression(args["StartMoney"], @" AND ca.[Qkje] >= @StartMoney ")
                            }
                            {
                              SqlExpressionHelp.GetDecimalExpression(args["EndMoney"], @" AND ca.[Qkje] <= @EndMoney ")
                            }
                            ORDER BY Jdsj DESC";
            return sql;
        }
        public JsonResult Getdata(MVCRequest request, string formJson)
        {
            MVCPagerData data = new MVCPagerData();
            if (!string.IsNullOrEmpty(formJson))
            {
                IDictionary<string, object> args = Getargs(formJson);
                MVCQueryHelp help = MVCQueryHelpFactory.Create();
                data = help.GetPagerData(request, args, GetSql(args));
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public IDictionary<string, object> Getargs(string formJson)
        {
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            IDictionary<string, object> args = jsonSerialize.Deserialize<Dictionary<string, object>>(formJson);
            //DateTime BeginJdsj, EndJdsj;
            //if (DateTime.TryParse(args["BeginJdsj"] + string.Empty, out BeginJdsj))
            //{
            //    args["BeginJdsj"] = BeginJdsj;
            //}
            //else
            //{
            //    args["BeginJdsj"] = Convert.ToDateTime("1901-1-1");
            //}
            //if (DateTime.TryParse(args["EndJdsj"] + string.Empty, out EndJdsj))
            //{
            //    args["EndJdsj"] = EndJdsj;
            //}
            //else
            //{
            //    args["EndJdsj"] = Convert.ToDateTime("9999-1-1");
            //}
            return args;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}