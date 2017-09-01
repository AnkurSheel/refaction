using System;
using System.Collections.Generic;
using System.Data;
using Refactor_me.Data;

namespace Refactor_me.Models
{
    public class Products
    {
        public Products()
        {
            LoadProducts(null);
        }

        public Products(string name)
        {
            LoadProducts(name);
        }

        public List<Product> Items { get; private set; }

        private void LoadProducts(string name)
        {
            Items = new List<Product>();
            using (var connection = Helpers.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select id from product";
                    if (!string.IsNullOrEmpty(name))
                    {
                        command.CommandText += @" where lower(name) like @name";
                        CommandExtensions.AddParameter(command, "name", "%" + name.ToLower() + "%");
                    }

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = Guid.Parse(reader["id"].ToString());
                        Items.Add(new Product(id));
                    }
                }
            }
        }
    }
}
