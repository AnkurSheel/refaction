using System;
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

        public Products GetAllProducts()
        {
            return ProductData.QueryAll();
        }

        public Product GetProduct(Guid id)
        {
            return ProductData.Query(id);
        }

        public Products GetProducts(string name)
        {
            return ProductData.Query(name);
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
