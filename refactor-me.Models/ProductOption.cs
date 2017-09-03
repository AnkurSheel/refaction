using System;

namespace Refactor_me.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid ProductId { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) || obj.GetType() != this.GetType())
            {
                return false;
            }

            var other = (ProductOption)obj;
            return Id.Equals(other.Id) && ProductId.Equals(other.ProductId) && string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description);
        }
    }
}
