#nullable disable

namespace EF.Practice.DatabaseFirst.Entities
{
    public partial class SalesPerson
    {
        public int SalesPersonId { get; set; }

        public int BusinessEntityId { get; set; }

        public int TerritoryId { get; set; }

        public int SalesQuota { get; set; }

        public virtual Employee BusinessEntity { get; set; }
    }
}
