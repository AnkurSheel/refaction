using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Controllers;
using refactor_me.Models;
using System.Web.Http;

namespace refactor_me.Tests
{
    [TestClass]
    public class ProductControllerTests
    {
        private readonly Guid TestProductId1 = new Guid("11111111-1111-1111-1111-111111111111");
        private readonly Guid TestProductId2 = new Guid("22222222-2222-2222-2222-222222222222");
        private readonly Guid TestProductId3 = new Guid("33333333-3333-3333-3333-333333333333");
        private readonly Guid TestProductInvalidId = new Guid("44444444-4444-4444-4444-444444444444");
        private readonly string ProductName1 = "Name1";
        private readonly string ProductName2 = "Name2";
        private readonly string ProductName3 = "Name3";

        private ProductsController _productsController;
        private IList<Product> _testProducts;
        private Product _newProduct;

        public ProductControllerTests()
        {
            _productsController = new ProductsController();
        }

        [TestInitialize]
        public void Setup()
        {
            // TODO : USE SQL To populate the table
            var allProducts = _productsController.GetAll();
            foreach (var product in allProducts.Items)
            {
                _productsController.Delete(product.Id);
            }

            SetupTestData();
            foreach (var product in _testProducts)
            {
                _productsController.Create(product);
            }
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenGettingAllProducts()
        {
            var products = _productsController.GetAll();

            Assert.AreEqual(_testProducts.Count, products.Items.Count);

            foreach (var product in products.Items)
            {
                var expect = _testProducts.AsQueryable().FirstOrDefault(p => p.Id == product.Id);
                Assert.AreEqual(expect, product);
            }
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallSearchByNameAndProductExists()
        {
            var result = _productsController.SearchByName(ProductName1);
            Assert.AreEqual(1, result.Items.Count);

            var product = result.Items.ElementAt(0);
            var expect = _testProducts.AsQueryable().FirstOrDefault(p => p.Id == product.Id);
            Assert.AreEqual(expect, product);
        }

        [TestMethod]
        public void ShouldGetNoDataWhenCallSearchByNameAndProductDoesNotExist()
        {
            var result = _productsController.SearchByName("Invalid Name");

            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallGetProductAndProductExists()
        {
            var product = _productsController.GetProduct(TestProductId1);

            var expect = _testProducts.AsQueryable().FirstOrDefault(p => p.Id == product.Id);
            Assert.AreEqual(expect, product);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetProductAndProductDoesNotExist()
        {
            _productsController.GetProduct(TestProductInvalidId);
        }

        [TestMethod]
        public void ShouldCreateDataWhenCallCreateAndProductDoesNotExist()
        {
            _productsController.Create(_newProduct);

            var result = _productsController.GetProduct(_newProduct.Id);
            Assert.AreEqual(_newProduct, result);
        }

        [TestMethod]
        public void ShouldUpdateDataWhenCallCreateAndProductExists()
        {
            var oldProduct = _productsController.GetProduct(TestProductId1);
            oldProduct.Name = "NewName1";

            _productsController.Create(oldProduct);

            var result = _productsController.GetProduct(TestProductId1);

            Assert.AreEqual(oldProduct, result);
        }

        [TestMethod]
        public void ShouldUpdateDataWhenWhenCallUpdateAndProductExists()
        {
            var oldProduct = _productsController.GetProduct(TestProductId1);
            _newProduct.Id = oldProduct.Id;

            _productsController.Update(oldProduct.Id, _newProduct);

            var result = _productsController.GetProduct(TestProductId1);

            Assert.AreEqual(_newProduct, result);
        }

        [TestMethod]
        public void ShouldDoNothingWhenWhenCallUpdateAndProductDoesNotExist()
        {
            _productsController.Update(_newProduct.Id, _newProduct);

            var result = _productsController.GetAll();

            Assert.AreEqual(_testProducts.Count, result.Items.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallDeleteAndProductDoesNotExist()
        {
            _productsController.Delete(_newProduct.Id);

            var result = _productsController.GetAll();

            Assert.AreEqual(_testProducts.Count, result.Items.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldDeleteProductAndAllItsOptionsWhenCallDeleteAndProductExists()
        {
            _productsController.Delete(TestProductId1);

            var resultOptions = _productsController.GetOptions(TestProductId1);

            _productsController.GetProduct(TestProductId1);
            Assert.AreEqual(0, resultOptions.Items.Count);
        }



        private void SetupTestData()
        {
            _newProduct = new Product
                          {
                              Id = TestProductId3,
                              Name = ProductName3,
                              Description = "Description3",
                              Price = 4,
                              DeliveryPrice = 4
                          };

            _testProducts = new List<Product>
                   {
                       new Product
                       {
                           Id = TestProductId1,
                           Name = ProductName1,
                           Description = "Description1",
                           DeliveryPrice = 1,
                           Price = 2
                       },
                       new Product
                       {
                           Id = TestProductId2,
                           Name = ProductName2,
                           Description = "Description2",
                           DeliveryPrice = 3,
                           Price = 4
                       }
                   };
        }
    }
}
