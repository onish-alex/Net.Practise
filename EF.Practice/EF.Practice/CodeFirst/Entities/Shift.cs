namespace EF.Practice.CodeFirst.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Shift", Schema = "HumanResources")]
    public class Shift
    {
        public int ShiftId { get; set; }

        public string Name { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public List<EmployeeDepartmentShift> EmployeeDepartmentShifts { get; set; }
    }
}
