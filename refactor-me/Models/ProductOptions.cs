using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Refactor_me.Models
{
    public class ProductOptions
    {
        public ProductOptions()
        {
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions($"where productid = '{productId}'");
        }

        public List<ProductOption> Items { get; private set; }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select id from productoption {where}", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new ProductOption(id));
            }

            conn.Close();
        }
    }
}
