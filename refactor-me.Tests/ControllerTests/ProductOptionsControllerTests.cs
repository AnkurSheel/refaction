using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactor_me.Models;

namespace refactor_me.Tests.ControllerTests
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
                _productsController.Create(product);
            }

            foreach (var productOptions in _productOptionsData)
            {
                _productOptionsController.Create(productOptions.ProductId, productOptions);
            }
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallGetOptionsAndProductExists()
        {
            var options = _productOptionsController.GetOptions(ProductId1).ToList();

            Assert.AreEqual((object)_productOptionsData.AsQueryable().Count(po => po.ProductId == ProductId1), options.Count);

            foreach (var option in options)
            {
                var expect = _productOptionsData.AsQueryable().FirstOrDefault(po => po.Id == option.Id);
                Assert.AreEqual((object)expect, option);
            }
        }

        [TestMethod]
        public void ShouldGetNothingWhenCallGetOptionsAndProductDoesNotExist()
        {
            var options = _productOptionsController.GetOptions(InvalidId).ToList();

            Assert.AreEqual(0, options.Count);
        }

        [TestMethod]
        public void ShouldGetCorrectDataWhenCallGetOptionAndProductExistsAndProductOptionExists()
        {
            var result = _productOptionsController.GetOption(ProductId1, _productOptionId1);

            var expect = _productOptionsData.AsQueryable().FirstOrDefault(p => p.Id == result.Id);
            Assert.AreEqual((object)expect, result);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetOptionAndProductExistsAndProductOptionDoesNotExist()
        {
            _productOptionsController.GetOption(ProductId1, InvalidId);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetOptionAndProductDoesNotExistsAndProductOptionExists()
        {
            _productOptionsController.GetOption(InvalidId, ProductId1);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowExceptionWhenCallGetOptionAndProductDoesNotExistsAndProductOptionDoesNotExists()
        {
            _productOptionsController.GetOption(InvalidId, InvalidId);
        }

        [TestMethod]
        public void ShouldCreateOptionWhenCallCreateOptionAndProductExistsAndProductOptionDoesNotExist()
        {
            var originalNumberofOptions = _productOptionsController.GetOptions(_newProductOption.ProductId).ToList().Count;
            _productOptionsController.Create(_newProductOption.ProductId, _newProductOption);

            var option = _productOptionsController.GetOption(_newProductOption.ProductId, _newProductOption.Id);
            var newNumberOfOptions = _productOptionsController.GetOptions(_newProductOption.ProductId).ToList().Count;
            Assert.AreEqual((object)_newProductOption, option);
            Assert.AreEqual(originalNumberofOptions + 1, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldUpdateOptionWhenCallCreateOptionAndProductExistsAndProductOptionExists()
        {
            var oldProductOption = _productOptionsController.GetOption(ProductId1, _productOptionId1);
            oldProductOption.Name = "NewOption";

            var originalNumberofOptions = _productOptionsController.GetOptions(_newProductOption.ProductId).ToList().Count;
            _productOptionsController.Create(oldProductOption.ProductId, oldProductOption);

            var option = _productOptionsController.GetOption(ProductId1, _productOptionId1);
            var newNumberOfOptions = _productOptionsController.GetOptions(_newProductOption.ProductId).ToList().Count;
            Assert.AreEqual((object)oldProductOption, option);
            Assert.AreEqual((object)originalNumberofOptions, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldUpdateOptionWhenCallCreateOptionAndProductDoesNotExistAndProductOptionExists()
        {
            var oldProductOption = _productOptionsController.GetOption(ProductId1, _productOptionId1);
            oldProductOption.Name = "NewOption";

            var originalNumberofOptions = _productOptionsController.GetOptions(_newProductOption.ProductId).ToList().Count;
            _productOptionsController.Create(InvalidId, oldProductOption);
            oldProductOption.ProductId = ProductId1;
            var option = _productOptionsController.GetOption(ProductId1, _productOptionId1);
            var newNumberOfOptions = _productOptionsController.GetOptions(_newProductOption.ProductId).ToList().Count;
            Assert.AreEqual((object)oldProductOption, option);
            Assert.AreEqual((object)originalNumberofOptions, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldCreateOptionWhenCallCreateOptionAndProductDoesNotExistAndProductOptionDoesNotExists()
        {
            _newProductOption.Id = InvalidId;
            _productOptionsController.Create(InvalidId, _newProductOption);

            _newProductOption.ProductId = InvalidId;

            var option = _productOptionsController.GetOption(InvalidId, _newProductOption.Id);
            var newNumberOfOptions = _productOptionsController.GetOptions(InvalidId).ToList().Count;
            Assert.AreEqual((object)_newProductOption, option);
            Assert.AreEqual(1, newNumberOfOptions);
        }

        [TestMethod]
        public void ShouldUpdateOptionWhenWhenCallUpdateOptionAndProductOptionExists()
        {
            var oldProductOption = _productOptionsController.GetOption(ProductId1, _productOptionId1);
            _newProductOption.Id = oldProductOption.Id;
            _newProductOption.ProductId = oldProductOption.ProductId;

            _productOptionsController.UpdateOption(oldProductOption.Id, _newProductOption);

            var result = _productOptionsController.GetOption(ProductId1, _productOptionId1);

            Assert.AreEqual((object)_newProductOption, result);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallUpdateAndProductOptionDoesNotExist()
        {
            _productOptionsController.UpdateOption(_newProductOption.Id, _newProductOption);

            var options = _productOptionsController.GetOptions(ProductId1).ToList();

            Assert.AreEqual((object)_productOptionsData.AsQueryable().Count(po => po.ProductId == ProductId1), options.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallDeleteOptionAndProductOptionDoesExists()
        {
            _productOptionsController.Delete(_productOptionId1);

            var options = _productOptionsController.GetOptions(ProductId1).ToList();

            Assert.AreEqual(_productOptionsData.AsQueryable().Count(po => po.ProductId == ProductId1) - 1, options.Count);
        }

        [TestMethod]
        public void ShouldDoNothingWhenCallDeleteOptionAndProductOptionDoesNotExist()
        {
            _productOptionsController.Delete(_newProductOption.Id);

            var options = _productOptionsController.GetOptions(ProductId1).ToList();

            Assert.AreEqual((object)_productOptionsData.AsQueryable().Count(po => po.ProductId == ProductId1), options.Count);
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
