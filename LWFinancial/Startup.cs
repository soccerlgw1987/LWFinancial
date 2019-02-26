using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LWFinancial.Startup))]
namespace LWFinancial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
