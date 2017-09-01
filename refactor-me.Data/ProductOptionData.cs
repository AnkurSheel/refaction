using System;
using System.Data;
using Refactor_me.Data.Helpers;
using Refactor_me.Models;

namespace Refactor_me.Data
{
    public static class ProductOptionData
    {
        public static void Create(Guid productId, ProductOption newOption)
        {
            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    var optionExists = Query(newOption.Id) != null;
                    if (optionExists)
                    {
                        Update(newOption.Id, newOption);
                    }
                    else
                    {
                        command.CommandText =
                            @"insert into productoption (id, productid, name, description) values (@id, @productId, @name, @description)";
                        CommandExtensions.AddParameter(command, "productId", productId);
                        CommandExtensions.AddParameter(command, "id", newOption.Id);
                        CommandExtensions.AddParameter(command, "name", newOption.Name);
                        CommandExtensions.AddParameter(command, "description", newOption.Description);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void Delete(Guid id)
        {
            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"delete from productoption where id = @id";
                    CommandExtensions.AddParameter(command, "id", id);
                    command.ExecuteReader();
                }
            }
        }

        public static ProductOption Query(Guid id)
        {
            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select * from productoption where id = @id";
                    CommandExtensions.AddParameter(command, "id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Map(reader);
                        }
                    }
                }
            }

            return null;
        }

        public static ProductOptions QueryAll(Guid productId)
        {
            var options = new ProductOptions();
            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select * from productoption where productid = @productId";
                    CommandExtensions.AddParameter(command, "productId", productId);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        options.Items.Add(Map(reader));
                    }
                }
            }

            return options;
        }

        public static void Update(Guid id, ProductOption updatedOption)
        {
            using (var connection = ConnectionCreator.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"update productoption set name = @name, description = @description where id = @id";

                    CommandExtensions.AddParameter(command, "id", id);
                    CommandExtensions.AddParameter(command, "name", updatedOption.Name);
                    CommandExtensions.AddParameter(command, "description", updatedOption.Description);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static ProductOption Map(IDataRecord record)
        {
            var productOption = new ProductOption
                                {
                                    Id = Guid.Parse(record["Id"].ToString()),
                                    ProductId = Guid.Parse(record["ProductId"].ToString()),
                                    Name = record["Name"].ToString(),
                                    Description = DBNull.Value == record["Description"] ? null : record["Description"].ToString()
                                };
            return productOption;
        }
    }
}
