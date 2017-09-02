using System;
using System.Collections.Generic;
using System.Linq;
using Refactor_me.Data.Interfaces;
using Refactor_me.Models;
using Refactor_me.Services.Interfaces;

namespace Refactor_me.Services.Services
{
    public class ProductOptionsService : IProductOptionsService
    {
        private readonly IRepository<ProductOption> _productOptionRepository;

        public ProductOptionsService(IRepository<ProductOption> productOptionRepository)
        {
            _productOptionRepository = productOptionRepository;
        }

        public void AddNewOption(Guid productId, ProductOption newOption)
        {
            var optionExists = _productOptionRepository.FindById(newOption.Id) != null;
            if (optionExists)
            {
                _productOptionRepository.Update(newOption.Id, newOption);
            }
            else
            {
                newOption.ProductId = productId;
                _productOptionRepository.Add(newOption);
            }
        }

        public ProductOption GetOption(Guid id)
        {
            return _productOptionRepository.FindById(id);
        }

        public IEnumerable<ProductOption> GetOptions(Guid productId)
        {
            return _productOptionRepository.FindAll().Where(o => o.ProductId == productId);
        }

        public void RemoveOption(Guid id)
        {
            _productOptionRepository.Remove(id);
        }

        public void UpdateOption(Guid id, ProductOption updatedOption)
        {
            _productOptionRepository.Update(id, updatedOption);
        }
    }
}
