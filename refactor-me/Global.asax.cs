using System.Web.Http;
using Refactor_me.Data.Helpers;
using Refactor_me.Data.Interfaces;
using Refactor_me.Data.Repositories;
using Refactor_me.Models;
using Refactor_me.Services.Interfaces;
using Refactor_me.Services.Services;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace Refactor_me
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register<IRepository<Product>, ProductRepository>(Lifestyle.Singleton);
            container.Register<IRepository<ProductOption>, ProductOptionRepository>(Lifestyle.Singleton);
            container.Register<IConnectionCreator, ConnectionCreator>(Lifestyle.Singleton);
            container.Register<IProductService, ProductService>(Lifestyle.Scoped);
            container.Register<IProductOptionsService, ProductOptionsService>(Lifestyle.Scoped);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
