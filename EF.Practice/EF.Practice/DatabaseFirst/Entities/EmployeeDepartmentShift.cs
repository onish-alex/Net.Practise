#nullable disable

namespace EF.Practice.DatabaseFirst.Entities
{
    using System;

    public partial class EmployeeDepartmentShift
    {
        public int BusinessEntityId { get; set; }

        public int DepartmentId { get; set; }

        public int ShiftId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual Employee BusinessEntity { get; set; }

        public virtual Department Department { get; set; }

        public virtual Shift Shift { get; set; }
    }
}
