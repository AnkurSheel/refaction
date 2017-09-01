using System;
using Refactor_me.Models;

namespace Refactor_me.Services
{
    public class ProductService : IProductService
    {
        public void AddNewProduct(Product product)
        {
            product.Save();
        }

        public Products GetAllProducts()
        {
            return new Products();
        }

        public Product GetProduct(Guid id)
        {
            var product = new Product(id);
            return product.IsNew ? null : product;
        }

        public Products GetProducts(string name)
        {
            return new Products(name);
        }

        public void RemoveProduct(Guid id)
        {
            var product = new Product(id);
            product.Delete();
        }

        public void UpdateProductForId(Guid id, Product updatedProduct)
        {
            var orig = new Product(id)
                       {
                           Name = updatedProduct.Name,
                           Description = updatedProduct.Description,
                           Price = updatedProduct.Price,
                           DeliveryPrice = updatedProduct.DeliveryPrice
                       };

            if (!orig.IsNew)
            {
                orig.Save();
            }
        }
    }
}
