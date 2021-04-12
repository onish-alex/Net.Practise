namespace EF.Practice.CodeFirst.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Department", Schema = "HumanResources")]
    public class Department
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        public List<EmployeeDepartmentShift> EmployeeDepartmentShifts { get; set; }
    }
}
