using System;
using System.Data;
using Refactor_me.Data.Helpers;
using Refactor_me.Data.Interfaces;
using Refactor_me.Models;

namespace Refactor_me.Data.Repositories
{
    public class ProductOptionRepository : BaseRepository<ProductOption>
    {
        public ProductOptionRepository(IConnectionCreator connectionCreator)
            : base(connectionCreator)
        {
        }

        protected override void Create(ProductOption newOption, IDbCommand command)
        {
            command.CommandText = @"insert into productoption (id, productid, name, description) values (@id, @productId, @name, @description)";
            CommandExtensions.AddParameter(command, "productId", newOption.ProductId);
            CommandExtensions.AddParameter(command, "id", newOption.Id);
            CommandExtensions.AddParameter(command, "name", newOption.Name);
            CommandExtensions.AddParameter(command, "description", newOption.Description);
            command.ExecuteNonQuery();
        }

        protected override void Delete(Guid id, IDbCommand command)
        {
            command.CommandText = @"delete from productoption where id = @id";
            CommandExtensions.AddParameter(command, "id", id);
            command.ExecuteReader();
        }

        protected override ProductOption FindById(Guid id, IDbCommand command)
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

            return null;
        }

        protected override string GetTableName()
        {
            return "productoption";
        }

        protected override ProductOption Map(IDataRecord record)
        {
            var description = record["Description"] == DBNull.Value ? null : record["Description"].ToString();
            var productOption = new ProductOption
                                {
                                    Id = Guid.Parse(record["Id"].ToString()),
                                    ProductId = Guid.Parse(record["ProductId"].ToString()),
                                    Name = record["Name"].ToString(),
                                    Description = description
                                };
            return productOption;
        }

        protected override void Update(Guid id, ProductOption updatedOption, IDbCommand command)
        {
            command.CommandText = @"update productoption set name = @name, description = @description where id = @id";

            CommandExtensions.AddParameter(command, "id", id);
            CommandExtensions.AddParameter(command, "name", updatedOption.Name);
            CommandExtensions.AddParameter(command, "description", updatedOption.Description);
            command.ExecuteNonQuery();
        }
    }
}
