using App.Core.Enumeration;

namespace App.Core.Entity
{
    public class Delivery : BaseEntity
    {
        public string Address { get; set; }

        public DeliveryType TypeDelivery { get; set; }
    }
}