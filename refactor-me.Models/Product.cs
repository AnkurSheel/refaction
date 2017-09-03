using System;

namespace Refactor_me.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Product)obj;
            return Id.Equals(other.Id) && string.Equals(Name, other.Name) && string.Equals(Description, other.Description) && Price == other.Price &&
                   DeliveryPrice == other.DeliveryPrice;
        }
    }
}
