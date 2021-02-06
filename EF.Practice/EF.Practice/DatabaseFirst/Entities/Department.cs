#nullable disable

namespace EF.Practice.DatabaseFirst.Entities
{
    using System.Collections.Generic;

    public partial class Department
    {
        public Department()
        {
            this.EmployeeDepartmentShifts = new HashSet<EmployeeDepartmentShift>();
        }

        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        public virtual ICollection<EmployeeDepartmentShift> EmployeeDepartmentShifts { get; set; }
    }
}
