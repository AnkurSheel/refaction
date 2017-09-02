using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace Refactor_me.IOC
{
    public static class IocConfig
    {
        public static Container IocContainer { get; private set; }

        public static void Initialise()
        {
            IocContainer = new Container();

            IocContainer.Options.AllowOverridingRegistrations = true;

            DataBootstrapper.Bootstrap(IocContainer);
            ServicesBootstrapper.Bootstrap(IocContainer, Lifestyle.Transient);

            IocContainer.Verify();
        }

        public static void InitialiseForWebApi(HttpConfiguration httpConfiguration)
        {
            IocContainer = new Container();

            IocContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            DataBootstrapper.Bootstrap(IocContainer);
            ServicesBootstrapper.Bootstrap(IocContainer, Lifestyle.Scoped);

            IocContainer.RegisterWebApiControllers(httpConfiguration);

            IocContainer.Verify();

            httpConfiguration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(IocContainer);
        }
    }
}
