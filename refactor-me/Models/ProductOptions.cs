using System;
using System.Collections.Generic;
using System.Data;

namespace Refactor_me.Models
{
    public class ProductOptions
    {
        private readonly IDbConnection _connection = Helpers.NewConnection();

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
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $"select id from productoption {where}";

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = Guid.Parse(reader["id"].ToString());
                    Items.Add(new ProductOption(id));
                }
            }

            _connection.Close();
        }
    }
}
