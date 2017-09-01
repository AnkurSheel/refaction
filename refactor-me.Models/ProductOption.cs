using System;
using Newtonsoft.Json;
using Refactor_me.Data;

namespace Refactor_me.Models
{
    public class ProductOption
    {
        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {
            IsNew = true;
            using (var connection = Helpers.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"select * from productoption where id = @id";
                    CommandExtensions.AddParameter(command, "id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IsNew = false;
                            Id = Guid.Parse(reader["Id"].ToString());
                            ProductId = Guid.Parse(reader["ProductId"].ToString());
                            Name = reader["Name"].ToString();
                            Description = DBNull.Value == reader["Description"] ? null : reader["Description"].ToString();
                        }
                    }
                }
            }
        }

        public string Description { get; set; }

        public Guid Id { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public string Name { get; set; }

        public Guid ProductId { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((ProductOption)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ ProductId.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }

        public void Delete()
        {
            using (var connection = Helpers.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"delete from productoption where id = @id";
                    CommandExtensions.AddParameter(command, "id", Id);
                    command.ExecuteReader();
                }
            }
        }

        public void Save()
        {
            using (var connection = Helpers.NewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    if (IsNew)
                    {
                        command.CommandText =
                            @"insert into productoption (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)";
                        CommandExtensions.AddParameter(command, "ProductId", ProductId);
                    }
                    else
                    {
                        command.CommandText = @"update productoption set name = @Name, description = @Description where id = @Id";
                    }

                    CommandExtensions.AddParameter(command, "Id", Id);
                    CommandExtensions.AddParameter(command, "Name", Name);
                    CommandExtensions.AddParameter(command, "Description", Description);
                    command.ExecuteNonQuery();
                }
            }
        }

        protected bool Equals(ProductOption other)
        {
            return Id.Equals(other.Id) && ProductId.Equals(other.ProductId) && string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description);
        }
    }
}
