using System;
using System.Collections;
using System.Collections.Generic;
using Refactor_me.Data;
using Refactor_me.Models;
using Refactor_me.Services.Interfaces;

namespace Refactor_me.Services.Services
{
    public class ProductService : IProductService
    {
        public void AddNewProduct(Product product)
        {
            ProductData.Create(product);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return ProductData.QueryAll(string.Empty);
        }

        public Product GetProduct(Guid id)
        {
            return ProductData.Query(id);
        }

        public IEnumerable<Product> GetProducts(string name)
        {
            return ProductData.QueryAll(name);
        }

        public void RemoveProduct(Guid id)
        {
            ProductData.Delete(id);
        }

        public void UpdateProductForId(Guid id, Product updatedProduct)
        {
            ProductData.Update(id, updatedProduct);
        }
    }
}
