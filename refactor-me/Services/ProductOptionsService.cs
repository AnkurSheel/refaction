using System;
using Refactor_me.Models;

namespace Refactor_me.Services
{
    public class ProductOptionsService : IProductOptionsService
    {
        public void AddNewOption(Guid productId, ProductOption newOption)
        {
            newOption.ProductId = productId;
            newOption.Save();
        }

        public ProductOption GetOption(Guid id)
        {
            var option = new ProductOption(id);
            return option.IsNew ? null : option;
        }

        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        public void RemoveOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }

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
