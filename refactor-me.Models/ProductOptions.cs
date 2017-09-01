using System;
using System.Collections.Generic;

namespace Refactor_me.Models
{
    public class ProductOptions
    {
        public ProductOptions()
        {
            Items = new List<ProductOption>();
        }

        public List<ProductOption> Items { get; private set; }
    }
}
