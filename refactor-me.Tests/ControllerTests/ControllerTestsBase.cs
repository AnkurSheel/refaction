using System;
using System.Collections.Generic;
using Refactor_me;
using Refactor_me.Controllers;
using Refactor_me.Models;

namespace refactor_me.Tests.ControllerTests
{
    public class ControllerTestsBase
    {
        protected const string ProductName1 = "Name1";
        protected readonly Guid InvalidId = new Guid("44444444-4444-4444-4444-444444444444");
        protected readonly Guid ProductId1 = new Guid("11111111-1111-1111-1111-111111111111");
        protected readonly Guid ProductId2 = new Guid("22222222-2222-2222-2222-222222222222");
        protected readonly Guid ProductId3 = new Guid("33333333-3333-3333-3333-333333333333");
        protected readonly ProductOptionsController _productOptionsController;
        protected readonly ProductsController _productsController;

        protected IList<Product> ProductsData;

        protected ControllerTestsBase()
        {
            IocConfig.InitialiseIoc();
            _productsController = IocConfig.IocContainer.GetInstance<ProductsController>();
            _productOptionsController = IocConfig.IocContainer.GetInstance<ProductOptionsController>();
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
            var products = _productsController.GetAll();
            foreach (var product in products)
            {
                _productsController.Delete(product.Id);
            }
        }
    }
}
