using System;
using Refactor_me.Models;

namespace Refactor_me.Services.Interfaces
{
    public interface IProductService
    {
        void AddNewProduct(Product product);

        Products GetAllProducts();

        Product GetProduct(Guid id);

        Products GetProducts(string name);

        void RemoveProduct(Guid id);

        void UpdateProductForId(Guid id, Product updatedProduct);
    }
}
