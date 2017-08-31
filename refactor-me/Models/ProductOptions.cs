using System;
using System.Collections.Generic;
using System.Data;
using Refactor_me.DataHelpers;

namespace Refactor_me.Models
{
    public class ProductOptions
    {
        private readonly IDbConnection _connection = Helpers.NewConnection();

        // TODO : Remove this
        public ProductOptions()
        {
            LoadProductOptions(Guid.Empty);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions(productId);
        }

        public List<ProductOption> Items { get; private set; }

        private void LoadProductOptions(Guid productId)
        {
            Items = new List<ProductOption>();
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"select id from productoption";
                if (productId != Guid.Empty)
                {
                    command.CommandText += @" where productid = @productId";
                    CommandExtensions.AddParameter(command, "productId", productId);
                }

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
