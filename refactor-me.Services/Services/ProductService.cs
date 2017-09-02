using System;
using System.Collections.Generic;
using System.Linq;
using Refactor_me.Data;
using Refactor_me.Models;
using Refactor_me.Services.Interfaces;

namespace Refactor_me.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }

        public void AddNewProduct(Product product)
        {
            var productExists = _productRepository.FindById(product.Id) != null;
            if (productExists)
            {
                _productRepository.Update(product.Id, product);
            }
            else
            {
                _productRepository.Add(product);
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.FindAll();
        }

        public Product GetProduct(Guid id)
        {
            return _productRepository.FindById(id);
        }

        public IEnumerable<Product> GetProducts(string name)
        {
            return _productRepository.FindAll().Where(p => string.Compare(p.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public void RemoveProduct(Guid id)
        {
            var options = ProductOptionData.QueryAll(id);
            foreach (var option in options)
            {
                ProductOptionData.Delete(option.Id);
            }

            _productRepository.Remove(id);
        }

        public void UpdateProductForId(Guid id, Product updatedProduct)
        {
            var productExists = _productRepository.FindById(id) != null;
            if (!productExists)
            {
                return;
            }

            _productRepository.Update(id, updatedProduct);
        }
    }
}
