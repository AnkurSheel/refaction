using System;
using System.Collections.Generic;
using Refactor_me.Controllers;
using Refactor_me.Models;

namespace Refactor_me.Tests
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
            ProductsController = new ProductsController();
            ProductOptionsController = new ProductOptionsController();
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
            var allProducts = ProductsController.GetAll();
            foreach (var product in allProducts.Items)
            {
                ProductsController.Delete(product.Id);
            }
        }
    }
}
