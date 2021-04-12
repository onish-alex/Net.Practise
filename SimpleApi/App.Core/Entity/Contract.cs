using System;
using System.Collections.Generic;

namespace App.Core.Entity
{
    public class Contract : BaseEntity
    {
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public DateTime Date { get; set; }

        public int DeliveryId { get; set; }

        public Delivery Delivery { get; set; }
    }
}