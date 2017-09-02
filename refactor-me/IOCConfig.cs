using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace Refactor_me
{
    public static class IocConfig
    {
        public static Container IocContainer { get; private set; }

        public static void InitialiseIoc()
        {
            IocContainer = new Container();

            IocContainer.Options.AllowOverridingRegistrations = true;

            DataBootstrapper.Bootstrap(IocContainer);
            ServicesBootstrapper.Bootstrap(IocContainer, Lifestyle.Transient);

            IocContainer.Verify();
        }

        public static void InitialiseIocForWebApi()
        {
            IocContainer = new Container();

            IocContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            DataBootstrapper.Bootstrap(IocContainer);
            ServicesBootstrapper.Bootstrap(IocContainer, Lifestyle.Scoped);

            IocContainer.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            IocContainer.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(IocContainer);
        }
    }
}
