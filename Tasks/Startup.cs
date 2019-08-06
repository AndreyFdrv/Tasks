using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tasks.Web.Startup))]
namespace Tasks.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
