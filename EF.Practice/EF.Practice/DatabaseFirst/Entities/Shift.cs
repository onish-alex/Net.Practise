#nullable disable

namespace EF.Practice.DatabaseFirst.Entities
{
    using System;
    using System.Collections.Generic;

    public partial class Shift
    {
        public Shift()
        {
            this.EmployeeDepartmentShifts = new HashSet<EmployeeDepartmentShift>();
        }

        public int ShiftId { get; set; }

        public string Name { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public virtual ICollection<EmployeeDepartmentShift> EmployeeDepartmentShifts { get; set; }
    }
}
