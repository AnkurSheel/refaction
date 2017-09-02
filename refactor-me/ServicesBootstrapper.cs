using Refactor_me.Services.Interfaces;
using Refactor_me.Services.Services;
using SimpleInjector;

namespace Refactor_me
{
    public static class ServicesBootstrapper
    {
        public static void Bootstrap(Container container, Lifestyle lifestyle)
        {
            container.Register<IProductService, ProductService>(lifestyle);
            container.Register<IProductOptionsService, ProductOptionsService>(lifestyle);
        }
    }
}
