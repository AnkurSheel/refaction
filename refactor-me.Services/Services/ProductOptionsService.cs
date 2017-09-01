using System;
using Refactor_me.Data;
using Refactor_me.Models;
using Refactor_me.Services.Interfaces;

namespace Refactor_me.Services.Services
{
    public class ProductOptionsService : IProductOptionsService
    {
        public void AddNewOption(Guid productId, ProductOption newOption)
        {
            ProductOptionData.Create(productId, newOption);
        }

        public ProductOption GetOption(Guid id)
        {
            return ProductOptionData.Query(id);
        }

        public ProductOptions GetOptions(Guid productId)
        {
            return ProductOptionData.QueryAll(productId);
        }

        public void RemoveOption(Guid id)
        {
            ProductOptionData.Delete(id);
        }

        public void UpdateOption(Guid id, ProductOption updatedOption)
        {
            ProductOptionData.Update(id, updatedOption);
        }
    }
}
