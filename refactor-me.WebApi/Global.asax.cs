using System.Web.Http;
using Refactor_me.IOC;

namespace Refactor_me.WebApi
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            IocConfig.InitialiseForWebApi(GlobalConfiguration.Configuration);
        }
    }
}
