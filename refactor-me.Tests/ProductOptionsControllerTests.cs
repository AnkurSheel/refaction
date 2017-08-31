using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Models;

namespace refactor_me.Tests
{
    [TestClass]
    public class ProductOptionsControllerTests : ControllerTestsBase
    {
        private readonly Guid _productOptionId1 = new Guid("55555555-5555-5555-5555-555555555555");
        private readonly Guid _productOptionId2 = new Guid("66666666-6666-6666-6666-666666666666");
        private readonly Guid _productOptionId3 = new Guid("77777777-7777-7777-7777-777777777777");
        private readonly Guid _productOptionId4 = new Guid("88888888-8888-8888-8888-888888888888");
        private readonly Guid _productOptionId5 = new Guid("99999999-9999-9999-9999-999999999999");

        private IList<ProductOption> _productOptionsData;
        private ProductOption _newProductOption;

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

            foreach (var productOptions in _productOptionsData)
            {
                ProductsController.CreateOption(productOptions.ProductId, productOptions);
            }
        }


        [TestMethod]
        public void ShouldGetCorrectDataWhenCallGetOptionsAndProductExists()
        {
            var result = ProductsController.GetOptions(ProductId1);

            Assert.AreEqual((object)_productOptionsData.AsQueryable().Count(po => po.ProductId == ProductId1), result.Items.Count);

            foreach (var productOptions in result.Items)
            {
                var expect = _productOptionsData.AsQueryable().FirstOrDefault(po => po.Id == productOptions.Id);
                Assert.AreEqual((object)expect, productOptions);
            }
        }

        [TestMethod]
        public void ShouldGetNothingWhenCallGetOptionsAndProductDoesNotExist()
        {
            var result = ProductsController.GetOptions(InvalidId);

            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallGetOptionAndProductExistsAndProductOptionExists()
        {
            var result = ProductsController.GetOption(ProductId1, _productOptionId1);

            var expect = _productOptionsData.AsQueryable().FirstOrDefault(p => p.Id == result.Id);
            Assert.AreEqual((object)expect, result);

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetOptionAndProductExistsAndProductOptionDoesNotExist()
        {
            ProductsController.GetOption(ProductId1, InvalidId);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetOptionAndProductDoesNotExistsAndProductOptionExists()
        {
            ProductsController.GetOption(InvalidId, ProductId1);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetOptionAndProductDoesNotExistsAndProductOptionDoesNotExists()
        {
            ProductsController.GetOption(InvalidId, InvalidId);
        }

        [TestMethod]
        public void ShouldCreateOptionWhenCallCreateOptionAndProductExistsAndProductOptionDoesNotExist()
        {
            var originalNumberofOptions = ProductsController.GetOptions(_newProductOption.ProductId).Items.Count;
            ProductsController.CreateOption(_newProductOption.ProductId, _newProductOption);

            var option = ProductsController.GetOption(_newProductOption.ProductId, _newProductOption.Id);
            var newNumberOfOptions = ProductsController.GetOptions(_newProductOption.ProductId).Items.Count;
            Assert.AreEqual((object)_newProductOption, option);
            Assert.AreEqual(originalNumberofOptions + 1, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldUpdateOptionWhenCallCreateOptionAndProductExistsAndProductOptionExists()
        {
            var oldProductOption = ProductsController.GetOption(ProductId1, _productOptionId1);
            oldProductOption.Name = "NewOption";

            var originalNumberofOptions = ProductsController.GetOptions(_newProductOption.ProductId).Items.Count;
            ProductsController.CreateOption(oldProductOption.ProductId, oldProductOption);

            var option = ProductsController.GetOption(ProductId1, _productOptionId1);
            var newNumberOfOptions = ProductsController.GetOptions(_newProductOption.ProductId).Items.Count;
            Assert.AreEqual((object)oldProductOption, option);
            Assert.AreEqual((object)originalNumberofOptions, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldUpdateOptionWhenCallCreateOptionAndProductDoesNotExistAndProductOptionExists()
        {
            var oldProductOption = ProductsController.GetOption(ProductId1, _productOptionId1);
            oldProductOption.Name = "NewOption";

            var originalNumberofOptions = ProductsController.GetOptions(_newProductOption.ProductId).Items.Count;
            ProductsController.CreateOption(InvalidId, oldProductOption);
            oldProductOption.ProductId = ProductId1;
            var option = ProductsController.GetOption(ProductId1, _productOptionId1);
            var newNumberOfOptions = ProductsController.GetOptions(_newProductOption.ProductId).Items.Count;
            Assert.AreEqual((object)oldProductOption, option);
            Assert.AreEqual((object)originalNumberofOptions, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldCreateOptionWhenCallCreateOptionAndProductDoesNotExistAndProductOptionDoesNotExists()
        {
            _newProductOption.Id = InvalidId;
            ProductsController.CreateOption(InvalidId, _newProductOption);

            var option = ProductsController.GetOption(InvalidId, _newProductOption.Id);
            var newNumberOfOptions = ProductsController.GetOptions(InvalidId).Items.Count;
            Assert.AreEqual((object)_newProductOption, option);
            Assert.AreEqual(1, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldUpdateOptionWhenWhenCallUpdateOptionAndProductOptionExists()
        {
            var oldProductOption = ProductsController.GetOption(ProductId1, _productOptionId1);
            _newProductOption.Id = oldProductOption.Id;
            _newProductOption.ProductId = oldProductOption.ProductId;

            ProductsController.UpdateOption(oldProductOption.Id, _newProductOption);

            var result = ProductsController.GetOption(ProductId1, _productOptionId1);

            Assert.AreEqual((object)_newProductOption, result);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallUpdateAndProductOptionDoesNotExist()
        {
            ProductsController.UpdateOption(_newProductOption.Id, _newProductOption);

            var result = ProductsController.GetOptions(ProductId1);

            Assert.AreEqual((object)_productOptionsData.AsQueryable().Count(po => po.ProductId == ProductId1), result.Items.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallDeleteOptionAndProductOptionDoesExists()
        {
            ProductsController.DeleteOption(_productOptionId1);

            var resultOptions = ProductsController.GetOptions(ProductId1);

            Assert.AreEqual(_productOptionsData.AsQueryable().Count(po => po.ProductId == ProductId1) - 1,
                            resultOptions.Items.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallDeleteOptionAndProductOptionDoesNotExist()
        {
            ProductsController.DeleteOption(_newProductOption.Id);

            var result = ProductsController.GetOptions(ProductId1);

            Assert.AreEqual((object)_productOptionsData.AsQueryable().Count(po => po.ProductId == ProductId1), result.Items.Count);
        }

        protected override void SetupTestData()
        {
            base.SetupTestData();
            _newProductOption = new ProductOption
                                {
                                    Id = _productOptionId5,
                                    Name = "ProductOptionNew",
                                    Description = "ProductDescriptionNew",
                                    ProductId = ProductId1
                                };

            _productOptionsData = new List<ProductOption>()
                                  {
                                      new ProductOption
                                      {
                                          Id = _productOptionId1,
                                          ProductId = ProductId1,
                                          Name = "Option1",
                                          Description = "OptionDescription1"
                                      },
                                      new ProductOption
                                      {
                                          Id = _productOptionId2,
                                          ProductId = ProductId1,
                                          Name = "Option2",
                                          Description = "OptionDescription2"
                                      },
                                      new ProductOption
                                      {
                                          Id = _productOptionId3,
                                          ProductId = ProductId2,
                                          Name = "Option3",
                                          Description = "OptionDescription3"
                                      },
                                      new ProductOption
                                      {
                                          Id = _productOptionId4,
                                          ProductId = ProductId2,
                                          Name = "Option4",
                                          Description = "OptionDescription4"
                                      },
                                  };
        }
    }
}