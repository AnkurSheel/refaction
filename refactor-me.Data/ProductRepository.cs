using System;
using System.Data;
using Refactor_me.Data.Helpers;
using Refactor_me.Models;

namespace Refactor_me.Data
{
    public class ProductRepository : BaseRepository<Product>
    {
        protected override void Create(Product product, IDbCommand command)
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

        protected override void Delete(Guid id, IDbCommand command)
        {
            command.CommandText = @"delete from product where id = @id";
            CommandExtensions.AddParameter(command, "id", id);
            command.ExecuteNonQuery();
        }

        protected override Product FindById(Guid id, IDbCommand command)
        {
            command.CommandText = @"select * from product where id = @id";
            CommandExtensions.AddParameter(command, "id", id);

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return Map(reader);
            }

            return null;
        }

        protected override string GetTableName()
        {
            return "product";
        }

        protected override Product Map(IDataRecord record)
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

        protected override void Update(Guid id, Product updatedProduct, IDbCommand command)
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
