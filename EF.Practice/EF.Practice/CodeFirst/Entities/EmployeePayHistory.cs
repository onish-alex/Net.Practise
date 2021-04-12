namespace EF.Practice.CodeFirst.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeePayHistory", Schema = "HumanResources")]
    public class EmployeePayHistory
    {
        public int EmployeePayHistoryId { get; set; }

        public int BusinessEntityId { get; set; }

        public DateTime RateChangeDate { get; set; }

        public int Rate { get; set; }

        public int PayFrequency { get; set; }

        public Employee Employee { get; set; }
    }
}
