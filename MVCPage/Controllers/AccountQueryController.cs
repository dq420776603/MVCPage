using DAL.DB.MVCHelp;
using NHibernateHelp.MVCHelp;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebPageHelp;
using WebPageHelp.MVCHelp;

namespace MVCPage.Controllers
{
    public class AccountQueryController : Controller
    {
        // GET: AccountQuery
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
                                SqlExpressionHelp.GetExpression(Convert.ToString(args["customer"]), " AND ca.XyKehuId = @customer ")
                            }
                            {
                                SqlExpressionHelp.GetExpression(args["member"], " AND vc.RelationId = @member ")
                            }
                            {
                                SqlExpressionHelp.GetDateExpression(args["BeginJdsj"], " AND ipici.Jdsj >= @BeginJdsj ")
                            }
                            {
                                SqlExpressionHelp.GetDateExpression(args["EndJdsj"], " AND ipici.Jdsj <= @EndJdsj ")
                            }
                            {
                                SqlExpressionHelp.GetExpression(args["BranchID"], @" AND vc.RelationId IN (SELECT tUser.Id
													                                                        FROM dbo.Users tUser WITH(NOLOCK)
													                                                        WHERE tUser.CompanyId=@BranchID
												                                                           UNION ALL
												                                                           SELECT @BranchID) ")
                            }
                            {
                               SqlExpressionHelp.GetExpression(args["StateID"], @" AND vc.[State] = @StateID ")
                            }
                            {
                               SqlExpressionHelp.GetExpression(args["EtcCode"], @" AND ipici.[Code] LIKE isnull(@EtcCode,'')+'%' ")
                            }
                            {
                               SqlExpressionHelp.GetExpression(args["HandName"], @" AND pici.[Name] LIKE isnull(@HandName,'')+'%' ")
                            }
                            {
                               SqlExpressionHelp.GetExpression(args["Province"], @" AND ca.[Shengfen] LIKE isnull(@Province,'')+'%' ")
                            }
                            {
                               SqlExpressionHelp.GetExpression(args["City"], @" AND ca.[Chengshi] LIKE isnull(@City,'')+'%' ")
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
            IDictionary<string, object> args = Getargs(formJson);
            MVCQueryHelp help = MVCQueryHelpFactory.Create();
            MVCPagerData data = help.GetPagerData(request, args, GetSql(args));
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public IDictionary<string, object> Getargs(string formJson)
        {
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            IDictionary<string, object> args = jsonSerialize.Deserialize<Dictionary<string, object>>(formJson);
            DateTime BeginJdsj, EndJdsj;
            if (DateTime.TryParse(args["BeginJdsj"] + string.Empty, out BeginJdsj))
            {
                args["BeginJdsj"] = BeginJdsj;
            }
            else
            {
                args["BeginJdsj"] = Convert.ToDateTime("1901-1-1");
            }
            if (DateTime.TryParse(args["EndJdsj"] + string.Empty, out EndJdsj))
            {
                args["EndJdsj"] = EndJdsj;
            }
            else
            {
                args["EndJdsj"] = Convert.ToDateTime("9999-1-1");
            }
            return args;
        }
    }
}