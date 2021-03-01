namespace EF.Practice.CodeFirst.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Employee", Schema = "HumanResources")]
    public class Employee
    {
        [Key]
        public int BusinessEntityId { get; set; }

        public int NationalIDNumber { get; set; }

        public int LoginID { get; set; }

        public string OrganizationNode { get; set; }

        public int OrganizationLevel { get; set; }

        public string JobTitle { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public List<EmployeeDepartmentShift> EmployeeDepartmentShifts { get; set; }

        public List<JobCandidate> JobCandidates { get; set; }

        public List<SalesPerson> SalesPeople { get; set; }

        public List<EmployeePayHistory> History { get; set; }
    }
}
