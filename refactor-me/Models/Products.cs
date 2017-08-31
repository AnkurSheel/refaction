﻿using System;
using System.Collections.Generic;
using System.Data;

namespace Refactor_me.Models
{
    public class Products
    {
        private readonly IDbConnection _connection = Helpers.NewConnection();

        public Products()
        {
            LoadProducts(null);
        }

        public Products(string name)
        {
            LoadProducts($"where lower(name) like '%{name.ToLower()}%'");
        }

        public List<Product> Items { get; private set; }

        private void LoadProducts(string where)
        {
            Items = new List<Product>();
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $"select id from product {where}";

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = Guid.Parse(reader["id"].ToString());
                    Items.Add(new Product(id));
                }
            }

            _connection.Close();
        }
    }
}
