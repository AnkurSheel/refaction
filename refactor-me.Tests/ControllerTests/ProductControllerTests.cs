using System.Linq;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactor_me.Models;

namespace refactor_me.Tests.ControllerTests
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
                _productsController.Create(product);
            }
        }

        [TestMethod]
        public void ShouldCreateDataWhenCallCreateAndProductDoesNotExist()
        {
            _productsController.Create(_newProduct);

            var result = _productsController.GetProduct(_newProduct.Id);
            Assert.AreEqual(_newProduct, result);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldDeleteProductAndAllItsOptionsWhenCallDeleteAndProductExists()
        {
            _productsController.Delete(ProductId1);

            var options = _productOptionsController.GetOptions(ProductId1).ToList();

            Assert.IsNull(_productsController.GetProduct(ProductId1));
            Assert.AreEqual(0, options.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallDeleteAndProductDoesNotExist()
        {
            _productsController.Delete(_newProduct.Id);

            var products = _productsController.GetAll().ToList();

            Assert.AreEqual(ProductsData.Count, products.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenWhenCallUpdateAndProductDoesNotExist()
        {
            _productsController.Update(_newProduct.Id, _newProduct);

            var products = _productsController.GetAll().ToList();

            Assert.AreEqual(ProductsData.Count, products.Count);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallGetProductAndProductExists()
        {
            var product = _productsController.GetProduct(ProductId1);

            var expect = ProductsData.AsQueryable().FirstOrDefault(p => p.Id == product.Id);
            Assert.AreEqual(expect, product);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallSearchByNameAndProductExists()
        {
            var products = _productsController.SearchByName(ProductName1).ToList();
            Assert.AreEqual(1, products.Count);

            var product = products.ElementAt(0);
            var expect = ProductsData.AsQueryable().FirstOrDefault(p => p.Id == product.Id);
            Assert.AreEqual(expect, product);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenGettingAllProducts()
        {
            var products = _productsController.GetAll().ToList();

            Assert.AreEqual(ProductsData.Count, products.Count);

            foreach (var product in products)
            {
                var expect = ProductsData.AsQueryable().FirstOrDefault(p => p.Id == product.Id);
                Assert.AreEqual(expect, product);
            }
        }

        [TestMethod]
        public void ShouldGetNoDataWhenCallSearchByNameAndProductDoesNotExist()
        {
            var products = _productsController.SearchByName("Invalid Name").ToList();

            Assert.AreEqual(0, products.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetProductAndProductDoesNotExist()
        {
            _productsController.GetProduct(InvalidId);
        }

        [TestMethod]
        public void ShouldUpdateDataWhenCallCreateAndProductExists()
        {
            var oldProduct = _productsController.GetProduct(ProductId1);
            oldProduct.Name = "NewName1";

            _productsController.Create(oldProduct);

            var result = _productsController.GetProduct(ProductId1);

            Assert.AreEqual(oldProduct, result);
        }

        [TestMethod]
        public void ShouldUpdateDataWhenWhenCallUpdateAndProductExists()
        {
            var oldProduct = _productsController.GetProduct(ProductId1);
            _newProduct.Id = oldProduct.Id;

            _productsController.Update(oldProduct.Id, _newProduct);

            var result = _productsController.GetProduct(ProductId1);

            Assert.AreEqual(_newProduct, result);
        }

        protected override void SetupTestData()
        {
            base.SetupTestData();
            _newProduct = new Product { Id = ProductId3, Name = "Name3", Description = "Description3", Price = 4, DeliveryPrice = 4 };
        }
    }
}
