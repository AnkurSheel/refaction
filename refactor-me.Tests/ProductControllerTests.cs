﻿using System.Linq;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactor_me.Models;

namespace Refactor_me.Tests
{
    [TestClass]
    public class ProductControllerTests : ControllerTestsBase
    {
        private Product _newProduct;

        [TestInitialize]
        public void Setup()
        {
            // TODO : USE SQL To populate the table
            ClearDatabase();
            SetupTestData();
            foreach (var product in ProductsData)
            {
                ProductsController.Create(product);
            }
        }

        [TestMethod]
        public void ShouldCreateDataWhenCallCreateAndProductDoesNotExist()
        {
            ProductsController.Create(_newProduct);

            var result = ProductsController.GetProduct(_newProduct.Id);
            Assert.AreEqual(_newProduct, result);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldDeleteProductAndAllItsOptionsWhenCallDeleteAndProductExists()
        {
            ProductsController.Delete(ProductId1);

            var resultOptions = ProductsController.GetOptions(ProductId1);

            ProductsController.GetProduct(ProductId1);
            Assert.AreEqual(0, resultOptions.Items.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallDeleteAndProductDoesNotExist()
        {
            ProductsController.Delete(_newProduct.Id);

            var result = ProductsController.GetAll();

            Assert.AreEqual(ProductsData.Count, result.Items.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenWhenCallUpdateAndProductDoesNotExist()
        {
            ProductsController.Update(_newProduct.Id, _newProduct);

            var result = ProductsController.GetAll();

            Assert.AreEqual(ProductsData.Count, result.Items.Count);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallGetProductAndProductExists()
        {
            var product = ProductsController.GetProduct(ProductId1);

            var expect = ProductsData.AsQueryable().FirstOrDefault(p => p.Id == product.Id);
            Assert.AreEqual(expect, product);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallSearchByNameAndProductExists()
        {
            var result = ProductsController.SearchByName(ProductName1);
            Assert.AreEqual(1, result.Items.Count);

            var product = result.Items.ElementAt(0);
            var expect = ProductsData.AsQueryable().FirstOrDefault(p => p.Id == product.Id);
            Assert.AreEqual(expect, product);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenGettingAllProducts()
        {
            var products = ProductsController.GetAll();

            Assert.AreEqual(ProductsData.Count, products.Items.Count);

            foreach (var product in products.Items)
            {
                var expect = ProductsData.AsQueryable().FirstOrDefault(p => p.Id == product.Id);
                Assert.AreEqual(expect, product);
            }
        }

        [TestMethod]
        public void ShouldGetNoDataWhenCallSearchByNameAndProductDoesNotExist()
        {
            var result = ProductsController.SearchByName("Invalid Name");

            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetProductAndProductDoesNotExist()
        {
            ProductsController.GetProduct(InvalidId);
        }

        [TestMethod]
        public void ShouldUpdateDataWhenCallCreateAndProductExists()
        {
            var oldProduct = ProductsController.GetProduct(ProductId1);
            oldProduct.Name = "NewName1";

            ProductsController.Create(oldProduct);

            var result = ProductsController.GetProduct(ProductId1);

            Assert.AreEqual(oldProduct, result);
        }

        [TestMethod]
        public void ShouldUpdateDataWhenWhenCallUpdateAndProductExists()
        {
            var oldProduct = ProductsController.GetProduct(ProductId1);
            _newProduct.Id = oldProduct.Id;

            ProductsController.Update(oldProduct.Id, _newProduct);

            var result = ProductsController.GetProduct(ProductId1);

            Assert.AreEqual(_newProduct, result);
        }

        protected override void SetupTestData()
        {
            base.SetupTestData();
            _newProduct = new Product { Id = ProductId3, Name = "Name3", Description = "Description3", Price = 4, DeliveryPrice = 4 };
        }
    }
}
