using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCPage.Startup))]
namespace MVCPage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
