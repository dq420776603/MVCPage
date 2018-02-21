using NHibernate;
using NHibernate.Cfg;
using Spring.Data.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.DB
{
    public class SpringDBManager
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static SpringDBManager instance = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static SpringDBManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpringDBManager();
                }
                return instance;
            }
        }
        /// <summary>
        /// Hibernate文件位置
        /// </summary>
        private string hibernateFile = string.Empty;
        /// <summary>
        /// Hibernate文件位置，第一次调用此属性时要 HttpContext.Current 不为null
        /// </summary>
        public virtual string HibernateFile
        {
            get
            {
                if (string.IsNullOrEmpty(hibernateFile))
                {
                    hibernateFile = HttpContext.Current.Server.MapPath(@"~/App_Data/configFile/NHibernate/hibernate.cfg.xml");
                }
                return hibernateFile;
            }
        }
        #region HibernateTemplate相关
        /// <summary>
        /// 获得HibernateTemplate
        /// </summary>
        /// <returns></returns>
        public HibernateTemplate GetHibernateTemplate()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Connstring"].ConnectionString;
            return GetHibernateTemplateByConnectionString(connectionString);
        }
        /// <summary>
        /// 获得HibernateTemplate
        /// </summary>
        /// <returns></returns>
        public HibernateTemplate GetHibernateTemplateByConnectionString(string connectionString)
        {
            Configuration config = new Configuration().Configure(HibernateFile);
            return GetHTByConfiguration(config, connectionString);
        }
        #endregion

        #region 根据NHibernate的Configuration获得HibernateTemplate
        public HibernateTemplate GetHTByConfiguration(Configuration config, string connectionString)
        {
            config.Properties[NHibernate.Cfg.Environment.ConnectionString] = connectionString;
            return GetHTByConfiguration(config);
        }
        public HibernateTemplate GetHTByConfiguration(Configuration config)
        {
            ISessionFactory sessionFactory2 = config.BuildSessionFactory();
            //HibernateTemplate hibernateTemplate2 = SpringConfig.ApplicationContext.GetObject("hibernateTemplate1") as HibernateTemplate;
            //hibernateTemplate2.SessionFactory = sessionFactory2;
            HibernateTemplate hibernateTemplate2 = new HibernateTemplate(sessionFactory2);
            return hibernateTemplate2;
        }
        #endregion
    }
}
