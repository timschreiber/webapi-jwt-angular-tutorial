using Microsoft.Owin.Cors;
using Owin;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Cors;

namespace WebApiJwtAngular.Web.Api
{
    public partial class Startup
    {
        public void ConfigureCors(IAppBuilder app)
        {
            var corsPolicy = new CorsPolicy
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
            };

            var corsOrigins = ConfigurationManager.AppSettings["CorsOrigins"];
            if (!string.IsNullOrWhiteSpace(corsOrigins))
            {
                if(corsOrigins == "*")
                {
                    corsPolicy.AllowAnyOrigin = true;
                }
                else
                {
                    corsPolicy.AllowAnyOrigin = false;
                    foreach (var origin in corsOrigins.Split(';'))
                    {
                        corsPolicy.Origins.Add(origin);
                    }
                }
            }

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(corsPolicy)
                }
            };

            app.UseCors(corsOptions);
        }
    }
}