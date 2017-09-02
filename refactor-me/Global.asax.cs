using System.Web.Http;

namespace Refactor_me
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            IocConfig.InitialiseIocForWebApi();
        }
    }
}
