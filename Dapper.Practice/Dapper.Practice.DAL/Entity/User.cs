namespace Dapper.Practice.DAL.Entity
{
    using System;

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
