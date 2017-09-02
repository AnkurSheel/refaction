using System;
using System.Collections.Generic;
using Refactor_me.Controllers;
using Refactor_me.Data.Helpers;
using Refactor_me.Data.Repositories;
using Refactor_me.Models;
using Refactor_me.Services.Services;

namespace refactor_me.Tests.ControllerTests
{
    public class ControllerTestsBase
    {
        protected const string ProductName1 = "Name1";
        protected readonly Guid InvalidId = new Guid("44444444-4444-4444-4444-444444444444");
        protected readonly Guid ProductId1 = new Guid("11111111-1111-1111-1111-111111111111");
        protected readonly Guid ProductId2 = new Guid("22222222-2222-2222-2222-222222222222");
        protected readonly Guid ProductId3 = new Guid("33333333-3333-3333-3333-333333333333");
        protected readonly ProductOptionsController ProductOptionsController;
        protected readonly ProductsController ProductsController;

        protected IList<Product> ProductsData;

        protected ControllerTestsBase()
        {
            var connectionCreator = new ConnectionCreator();
            var productRepository = new ProductRepository(connectionCreator);
            var productOptionRepository = new ProductOptionRepository(connectionCreator);
            var productService = new ProductService(productRepository, productOptionRepository);
            ProductsController = new ProductsController(productService);
            var productOptionsService = new ProductOptionsService(productOptionRepository);
            ProductOptionsController = new ProductOptionsController(productOptionsService);
        }

        protected virtual void SetupTestData()
        {
            ProductsData = new List<Product>
                           {
                               new Product
                               {
                                   Id = ProductId1,
                                   Name = ProductName1,
                                   Description = "Description1",
                                   DeliveryPrice = 1,
                                   Price = 2
                               },
                               new Product
                               {
                                   Id = ProductId2,
                                   Name = "Name2",
                                   Description = "Description2",
                                   DeliveryPrice = 3,
                                   Price = 4
                               }
                           };
        }

        protected void ClearDatabase()
        {
            var products = ProductsController.GetAll();
            foreach (var product in products)
            {
                ProductsController.Delete(product.Id);
            }
        }
    }
}
