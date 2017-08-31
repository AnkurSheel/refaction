using System;
using System.Data;
using Newtonsoft.Json;

namespace Refactor_me.Models
{
    public class ProductOption
    {
        private readonly IDbConnection _connection = Helpers.NewConnection();

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {
            IsNew = true;
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $"select * from productoption where id = '{id}'";

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

            _connection.Close();
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
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $"delete from productoption where id = '{Id}'";
                command.ExecuteReader();
            }

            _connection.Close();
        }

        public void Save()
        {
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                if (IsNew)
                {
                    command.CommandText =
                        $"insert into productoption (id, productid, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')";
                }
                else
                {
                    command.CommandText = $"update productoption set name = '{Name}', description = '{Description}' where id = '{Id}'";
                }

                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        protected bool Equals(ProductOption other)
        {
            return Id.Equals(other.Id) && ProductId.Equals(other.ProductId) && string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description);
        }
    }
}
