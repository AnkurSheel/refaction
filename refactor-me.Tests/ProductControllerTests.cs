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
        private readonly Guid InvalidId = new Guid("44444444-4444-4444-4444-444444444444");
        private readonly Guid TestProductOptionId1 = new Guid("55555555-5555-5555-5555-555555555555");
        private readonly Guid TestProductOptionId2 = new Guid("66666666-6666-6666-6666-666666666666");
        private readonly Guid TestProductOptionId3 = new Guid("77777777-7777-7777-7777-777777777777");
        private readonly Guid TestProductOptionId4 = new Guid("88888888-8888-8888-8888-888888888888");
        private readonly Guid TestProductOptionId5 = new Guid("99999999-9999-9999-9999-999999999999");
        private readonly string ProductName1 = "Name1";
        private readonly string ProductName2 = "Name2";
        private readonly string ProductName3 = "Name3";
        
        private ProductsController _productsController;
        private IList<Product> _testProducts;
        private IList<ProductOption> _testProductOptions;
        private Product _newProduct;
        private ProductOption _newProductOption;

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

            foreach (var productOptions in _testProductOptions)
            {
                _productsController.CreateOption(productOptions.ProductId, productOptions);
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
            _productsController.GetProduct(InvalidId);
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

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallGetOptionsAndProductExists()
        {
            var result = _productsController.GetOptions(TestProductId1);

            Assert.AreEqual(_testProductOptions.AsQueryable().Count(po => po.ProductId == TestProductId1), result.Items.Count);

            foreach (var productOptions in result.Items)
            {
                var expect = _testProductOptions.AsQueryable().FirstOrDefault(po => po.Id == productOptions.Id);
                Assert.AreEqual(expect, productOptions);
            }
        }

        [TestMethod]
        public void ShouldGetNothingWhenCallGetOptionsAndProductDoesNotExist()
        {
            var result = _productsController.GetOptions(InvalidId);

            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallGetOptionAndProductExistsAndProductOptionExists()
        {
            var result = _productsController.GetOption(TestProductId1, TestProductOptionId1);

            var expect = _testProductOptions.AsQueryable().FirstOrDefault(p => p.Id == result.Id);
            Assert.AreEqual(expect, result);

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetOptionAndProductExistsAndProductOptionDoesNotExist()
        {
            _productsController.GetOption(TestProductId1, InvalidId);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetOptionAndProductDoesNotExistsAndProductOptionExists()
        {
            _productsController.GetOption(InvalidId, TestProductId1);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetOptionAndProductDoesNotExistsAndProductOptionDoesNotExists()
        {
            _productsController.GetOption(InvalidId, InvalidId);
        }

        [TestMethod]
        public void ShouldCreateOptionWhenCallCreateOptionAndProductExistsAndProductOptionDoesNotExist()
        {
            var originalNumberofOptions = _productsController.GetOptions(_newProductOption.ProductId).Items.Count;
            _productsController.CreateOption(_newProductOption.ProductId, _newProductOption);

            var option = _productsController.GetOption(_newProductOption.ProductId, _newProductOption.Id);
            var newNumberOfOptions = _productsController.GetOptions(_newProductOption.ProductId).Items.Count;
            Assert.AreEqual(_newProductOption, option);
            Assert.AreEqual(originalNumberofOptions + 1, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldUpdateOptionWhenCallCreateOptionAndProductExistsAndProductOptionExists()
        {
            var oldProductOption = _productsController.GetOption(TestProductId1, TestProductOptionId1);
            oldProductOption.Name = "NewOption";

            var originalNumberofOptions = _productsController.GetOptions(_newProductOption.ProductId).Items.Count;
            _productsController.CreateOption(oldProductOption.ProductId, oldProductOption);

            var option = _productsController.GetOption(TestProductId1, TestProductOptionId1);
            var newNumberOfOptions = _productsController.GetOptions(_newProductOption.ProductId).Items.Count;
            Assert.AreEqual(oldProductOption, option);
            Assert.AreEqual(originalNumberofOptions, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldUpdateOptionWhenCallCreateOptionAndProductDoesNotExistAndProductOptionExists()
        {
            var oldProductOption = _productsController.GetOption(TestProductId1, TestProductOptionId1);
            oldProductOption.Name = "NewOption";

            var originalNumberofOptions = _productsController.GetOptions(_newProductOption.ProductId).Items.Count;
            _productsController.CreateOption(InvalidId, oldProductOption);
            oldProductOption.ProductId = TestProductId1;
            var option = _productsController.GetOption(TestProductId1, TestProductOptionId1);
            var newNumberOfOptions = _productsController.GetOptions(_newProductOption.ProductId).Items.Count;
            Assert.AreEqual(oldProductOption, option);
            Assert.AreEqual(originalNumberofOptions, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldCreateOptionWhenCallCreateOptionAndProductDoesNotExistAndProductOptionDoesNotExists()
        {
            _newProductOption.Id = InvalidId;
            _productsController.CreateOption(InvalidId, _newProductOption);

            var option = _productsController.GetOption(InvalidId, _newProductOption.Id);
            var newNumberOfOptions = _productsController.GetOptions(InvalidId).Items.Count;
            Assert.AreEqual(_newProductOption, option);
            Assert.AreEqual(1, newNumberOfOptions);
        }


        [TestMethod]
        public void ShouldUpdateOptionWhenWhenCallUpdateOptionAndProductOptionExists()
        {
            var oldProductOption = _productsController.GetOption(TestProductId1, TestProductOptionId1);
            _newProductOption.Id = oldProductOption.Id;
            _newProductOption.ProductId = oldProductOption.ProductId;

            _productsController.UpdateOption(oldProductOption.Id, _newProductOption);

            var result = _productsController.GetOption(TestProductId1, TestProductOptionId1);

            Assert.AreEqual(_newProductOption, result);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallUpdateAndProductOptionDoesNotExist()
        {
            _productsController.UpdateOption(_newProductOption.Id, _newProductOption);

            var result = _productsController.GetOptions(TestProductId1);

            Assert.AreEqual(_testProductOptions.AsQueryable().Count(po => po.ProductId == TestProductId1), result.Items.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallDeleteOptionAndProductOptionDoesExists()
        {
            _productsController.DeleteOption(TestProductOptionId1);

            var resultOptions = _productsController.GetOptions(TestProductId1);

            Assert.AreEqual(_testProductOptions.AsQueryable().Count(po => po.ProductId == TestProductId1) - 1,
                            resultOptions.Items.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallDeleteOptionAndProductOptionDoesNotExist()
        {
            _productsController.DeleteOption(_newProductOption.Id);

            var result = _productsController.GetOptions(TestProductId1);

            Assert.AreEqual(_testProductOptions.AsQueryable().Count(po => po.ProductId == TestProductId1), result.Items.Count);
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

            _newProductOption = new ProductOption
                                {
                                    Id = TestProductOptionId5,
                                    Name = "ProductOptionNew",
                                    Description = "ProductDescriptionNew",
                                    ProductId = TestProductId1
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

            _testProductOptions = new List<ProductOption>()
                                     {
                                         new ProductOption
                                         {
                                             Id = TestProductOptionId1,
                                             ProductId = TestProductId1,
                                             Name = "Option1",
                                             Description = "OptionDescription1"
                                         },
                                         new ProductOption
                                         {
                                             Id = TestProductOptionId2,
                                             ProductId = TestProductId1,
                                             Name = "Option2",
                                             Description = "OptionDescription2"
                                         },
                                         new ProductOption
                                         {
                                             Id = TestProductOptionId3,
                                             ProductId = TestProductId2,
                                             Name = "Option3",
                                             Description = "OptionDescription3"
                                         },
                                         new ProductOption
                                         {
                                             Id = TestProductOptionId4,
                                             ProductId = TestProductId2,
                                             Name = "Option4",
                                             Description = "OptionDescription4"
                                         },
                                     };
        }
    }
}
