using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApiJwtAngular.Web.Api.Startup))]

namespace WebApiJwtAngular.Web.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureCors(app);
            ConfigureAuth(app);
        }
    }
}