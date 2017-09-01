using System;
using Refactor_me.Models;

namespace Refactor_me.Services.Interfaces
{
    public interface IProductOptionsService
    {
        void AddNewOption(Guid productId, ProductOption newOption);

        ProductOption GetOption(Guid id);

        ProductOptions GetOptions(Guid productId);

        void RemoveOption(Guid id);

        void UpdateOption(Guid id, ProductOption updatedOption);
    }
}
