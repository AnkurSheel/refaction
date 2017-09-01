using System;
using System.Collections.Generic;
using Refactor_me.Models;

namespace Refactor_me.Services.Interfaces
{
    public interface IProductService
    {
        void AddNewProduct(Product product);

        IEnumerable<Product> GetAllProducts();

        Product GetProduct(Guid id);

        IEnumerable<Product> GetProducts(string name);

        void RemoveProduct(Guid id);

        void UpdateProductForId(Guid id, Product updatedProduct);
    }
}
