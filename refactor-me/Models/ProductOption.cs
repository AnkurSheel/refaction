using System;
using System.Data.SqlClient;
using Newtonsoft.Json;

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
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select * from productoption where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                conn.Close();
                return;
            }

            IsNew = false;
            Id = Guid.Parse(rdr["Id"].ToString());
            ProductId = Guid.Parse(rdr["ProductId"].ToString());
            Name = rdr["Name"].ToString();
            Description = DBNull.Value == rdr["Description"] ? null : rdr["Description"].ToString();
            conn.Close();
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
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = new SqlCommand($"delete from productoption where id = '{Id}'", conn);
            cmd.ExecuteReader();
            conn.Close();
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            var cmd = IsNew
                          ? new
                              SqlCommand($"insert into productoption (id, productid, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')",
                                         conn)
                          : new SqlCommand($"update productoption set name = '{Name}', description = '{Description}' where id = '{Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        protected bool Equals(ProductOption other)
        {
            return Id.Equals(other.Id) && ProductId.Equals(other.ProductId) && string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description);
        }
    }
}
