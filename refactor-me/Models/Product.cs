using System;
using System.Data;
using Newtonsoft.Json;

namespace Refactor_me.Models
{
    public class Product
    {
        private readonly IDbConnection _connection = Helpers.NewConnection();

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Product(Guid id)
        {
            IsNew = true;
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"select * from product where id = @id";
                Helpers.AddParameter(command, "id", id);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsNew = false;
                    Id = Guid.Parse(reader["Id"].ToString());
                    Name = reader["Name"].ToString();
                    Description = DBNull.Value == reader["Description"] ? null : reader["Description"].ToString();
                    Price = decimal.Parse(reader["Price"].ToString());
                    DeliveryPrice = decimal.Parse(reader["DeliveryPrice"].ToString());
                }
            }

            _connection.Close();
        }

        public decimal DeliveryPrice { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public string Name { get; set; }

        public decimal Price { get; set; }

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

            return Equals((Product)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                hashCode = (hashCode * 397) ^ DeliveryPrice.GetHashCode();
                return hashCode;
            }
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
            {
                option.Delete();
            }

            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"delete from product where id = @id";
                Helpers.AddParameter(command, "id", Id);
                command.ExecuteNonQuery();
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
                        @"insert into product (id, name, description, price, deliveryprice) values (@Id, @Name, @Description, @Price, @DeliveryPrice)";
                }
                else
                {
                    command.CommandText =
                        @"update product set name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice where id = @Id";
                }

                Helpers.AddParameter(command, "Id", Id);
                Helpers.AddParameter(command, "Name", Name);
                Helpers.AddParameter(command, "Description", Description);
                Helpers.AddParameter(command, "Price", Price);
                Helpers.AddParameter(command, "DeliveryPrice", DeliveryPrice);
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        protected bool Equals(Product other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name) && string.Equals(Description, other.Description) && Price == other.Price &&
                   DeliveryPrice == other.DeliveryPrice;
        }
    }
}
