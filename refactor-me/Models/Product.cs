using System;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Refactor_me.Models
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Product(Guid id)
        {
            IsNew = true;
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select * from product where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                return;
            }

            IsNew = false;
            Id = Guid.Parse(rdr["Id"].ToString());
            Name = rdr["Name"].ToString();
            Description = DBNull.Value == rdr["Description"] ? null : rdr["Description"].ToString();
            Price = decimal.Parse(rdr["Price"].ToString());
            DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());

            conn.Close();
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

            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = new SqlCommand($"delete from product where id = '{Id}'", conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            var cmd = IsNew
                          ? new
                              SqlCommand($"insert into product (id, name, description, price, deliveryprice) values ('{Id}', '{Name}', '{Description}', {Price}, {DeliveryPrice})",
                                         conn)
                          : new
                              SqlCommand($"update product set name = '{Name}', description = '{Description}', price = {Price}, deliveryprice = {DeliveryPrice} where id = '{Id}'",
                                         conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        protected bool Equals(Product other)
        {
            return Id.Equals(other.Id) && string.Equals(Name, other.Name) && string.Equals(Description, other.Description) && Price == other.Price &&
                   DeliveryPrice == other.DeliveryPrice;
        }
    }
}
