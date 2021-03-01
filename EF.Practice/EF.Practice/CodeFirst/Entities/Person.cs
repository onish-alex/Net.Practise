namespace EF.Practice.CodeFirst.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Person", Schema = "Person")]
    public class Person
    {
        public int PersonId { get; set; }

        public string PersonType { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
