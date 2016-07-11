using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BonContact.Web.Startup))]
namespace BonContact.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
