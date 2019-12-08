using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_1250865___MVC_Project.Startup))]
namespace _1250865___MVC_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
