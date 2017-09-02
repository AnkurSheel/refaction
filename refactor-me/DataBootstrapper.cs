using Refactor_me.Data.Helpers;
using Refactor_me.Data.Interfaces;
using Refactor_me.Data.Repositories;
using Refactor_me.Models;
using SimpleInjector;

namespace Refactor_me
{
    public static class DataBootstrapper
    {
        public static void Bootstrap(Container container)
        {
            container.Register<IConnectionCreator, ConnectionCreator>(Lifestyle.Singleton);

            container.Register<IRepository<Product>, ProductRepository>(Lifestyle.Singleton);
            container.Register<IRepository<ProductOption>, ProductOptionRepository>(Lifestyle.Singleton);
        }
    }
}
