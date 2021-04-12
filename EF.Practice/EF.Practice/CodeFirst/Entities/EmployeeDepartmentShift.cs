namespace EF.Practice.CodeFirst.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeDepartmentShift", Schema = "HumanResources")]
    public class EmployeeDepartmentShift
    {
        public int BusinessEntityId { get; set; }

        public int DepartmentId { get; set; }

        public int ShiftId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Department Department { get; set; }

        public Employee Employee { get; set; }

        public Shift Shift { get; set; }
    }
}
