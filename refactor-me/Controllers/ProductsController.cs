using System;
using System.Net;
using System.Web.Http;
using Refactor_me.Models;
using Refactor_me.Services;

namespace Refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;

        public ProductsController()
        {
            // TODO : This needs to be injected
            _productService = new ProductService();
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            _productService.AddNewProduct(product);
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            _productService.RemoveProduct(id);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }

        [Route]
        [HttpGet]
        public Products GetAll()
        {
            return _productService.GetAllProducts();
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return option;
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = _productService.GetProduct(id);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return product;
        }

        [Route]
        [HttpGet]
        public Products SearchByName(string name)
        {
            return _productService.GetProducts(name);
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product updatedProduct)
        {
            _productService.UpdateProductForId(id, updatedProduct);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption updatedOption)
        {
            var orig = new ProductOption(id) { Name = updatedOption.Name, Description = updatedOption.Description };

            if (!orig.IsNew)
            {
                orig.Save();
            }
        }
    }
}
