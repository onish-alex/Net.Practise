#nullable disable

namespace EF.Practice.DatabaseFirst.Entities
{
    using System.Collections.Generic;

    public partial class Employee
    {
        public Employee()
        {
            this.EmployeeDepartmentShifts = new HashSet<EmployeeDepartmentShift>();
            this.EmployeePayHistories = new HashSet<EmployeePayHistory>();
            this.JobCandidates = new HashSet<JobCandidate>();
            this.SalesPeople = new HashSet<SalesPerson>();
        }

        public int BusinessEntityId { get; set; }

        public int NationalIdnumber { get; set; }

        public int LoginId { get; set; }

        public string OrganizationNode { get; set; }

        public int OrganizationLevel { get; set; }

        public string JobTitle { get; set; }

        public int PersonId { get; set; }

        public virtual Person Person { get; set; }

        public virtual ICollection<EmployeeDepartmentShift> EmployeeDepartmentShifts { get; set; }

        public virtual ICollection<EmployeePayHistory> EmployeePayHistories { get; set; }

        public virtual ICollection<JobCandidate> JobCandidates { get; set; }

        public virtual ICollection<SalesPerson> SalesPeople { get; set; }
    }
}
