namespace EF.Practice.CodeFirst.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SalesPerson", Schema = "Sales")]
    public class SalesPerson
    {
        public int SalesPersonId { get; set; }

        public int BusinessEntityId { get; set; }

        public int TerritoryId { get; set; }

        public int SalesQuota { get; set; }

        public Employee Employee { get; set; }
    }
}
