#nullable disable

namespace EF.Practice.DatabaseFirst.Entities
{
    using System.Collections.Generic;

    public partial class Person
    {
        public Person()
        {
            this.Employees = new HashSet<Employee>();
        }

        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PersonType { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
