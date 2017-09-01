using System;
using System.Collections.Generic;
using System.Data;
using Refactor_me.Data.Helpers;
using Refactor_me.Models;

namespace Refactor_me.Data
{
    public static class ProductData
    {
        public static void Create(Product product)
        {
            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    var productExists = Query(product.Id) != null;
                    if (productExists)
                    {
                        Update(product.Id, product);
                    }
                    else
                    {
                        command.CommandText =
                            @"insert into product (id, name, description, price, deliveryprice) values (@id, @name, @description, @price, @deliveryPrice)";

                        CommandExtensions.AddParameter(command, "id", product.Id);
                        CommandExtensions.AddParameter(command, "name", product.Name);
                        CommandExtensions.AddParameter(command, "description", product.Description);
                        CommandExtensions.AddParameter(command, "price", product.Price);
                        CommandExtensions.AddParameter(command, "deliveryPrice", product.DeliveryPrice);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void Delete(Guid id)
        {
            var options = ProductOptionData.QueryAll(id);
            foreach (var option in options)
            {
                ProductOptionData.Delete(option.Id);
            }

            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"delete from product where id = @id";
                    CommandExtensions.AddParameter(command, "id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static Product Query(Guid id)
        {
            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select * from product where id = @id";
                    CommandExtensions.AddParameter(command, "id", id);

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return Map(reader);
                    }
                }
            }

            return null;
        }

        public static IEnumerable<Product> QueryAll(string name)
        {
            var products = new List<Product>();
            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select * from product";
                    if (!string.IsNullOrEmpty(name))
                    {
                        command.CommandText += @" where lower(name) like @name";
                        CommandExtensions.AddParameter(command, "name", "%" + name.ToLower() + "%");
                    }

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(Map(reader));
                    }
                }
            }

            return products;
        }

        public static void Update(Guid id, Product updatedProduct)
        {
            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    var productExists = Query(id) != null;
                    if (productExists)
                    {
                        command.CommandText =
                            @"update product set name = @name, description = @description, price = @price, deliveryprice = @deliveryPrice where id = @id";
                        CommandExtensions.AddParameter(command, "id", updatedProduct.Id);
                        CommandExtensions.AddParameter(command, "name", updatedProduct.Name);
                        CommandExtensions.AddParameter(command, "description", updatedProduct.Description);
                        CommandExtensions.AddParameter(command, "price", updatedProduct.Price);
                        CommandExtensions.AddParameter(command, "deliveryPrice", updatedProduct.DeliveryPrice);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static Product Map(IDataRecord record)
        {
            return new Product
                   {
                       Id = Guid.Parse(record["Id"].ToString()),
                       Name = record["Name"].ToString(),
                       Description = DBNull.Value == record["Description"] ? null : record["Description"].ToString(),
                       Price = decimal.Parse(record["Price"].ToString()),
                       DeliveryPrice = decimal.Parse(record["DeliveryPrice"].ToString())
                   };
        }
    }
}
