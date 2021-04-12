using System.Collections.Generic;

namespace App.Core.Entity
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public string Nds { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }

        public bool? Refrigerate { get; set; }

        public IEnumerable<Contract> Contracts { get; set; }

        public Item Clone()
        {
            return this.MemberwiseClone() as Item;
        }
    }
}