using System;
using System.Collections.Generic;

namespace Refactor_me.Models
{
    public class Products
    {
        public Products()
        {
            Items = new List<Product>();
        }

        public List<Product> Items { get; private set; }

    }
}
