#nullable disable

namespace EF.Practice.DatabaseFirst.Entities
{
    using System;

    public partial class EmployeePayHistory
    {
        public int EmployeePayHistoryId { get; set; }

        public int BusinessEntityId { get; set; }

        public DateTime RateChangeDate { get; set; }

        public int Rate { get; set; }

        public int PayFrequency { get; set; }

        public virtual Employee BusinessEntity { get; set; }
    }
}
