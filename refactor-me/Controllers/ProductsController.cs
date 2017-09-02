using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Refactor_me.Data.Repositories;
using Refactor_me.Models;
using Refactor_me.Services.Interfaces;
using Refactor_me.Services.Services;

namespace Refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;

        public ProductsController()
        {
            // TODO : This needs to be injected
            _productService = new ProductService(new ProductRepository(), new ProductOptionRepository());
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            _productService.AddNewProduct(product);
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            _productService.RemoveProduct(id);
        }

        [Route]
        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            return _productService.GetAllProducts();
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
        public IEnumerable<Product> SearchByName(string name)
        {
            return _productService.GetProducts(name);
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product updatedProduct)
        {
            _productService.UpdateProductForId(id, updatedProduct);
        }
    }
}
