namespace EF.Practice.Repository
{
    using System.Linq;
    using EF.Practice.CodeFirst.Contexts;
    using EF.Practice.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;

    public class PeopleRepository : EFRepository<Person>
    {
        public PeopleRepository(EmployeeDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<Person> SelectQuery => this.db.Set<Person>()
                                                               .Include(p => p.Employees);

        public override Person GetById(int id)
        {
            return this.SelectQuery.SingleOrDefault(x => x.PersonId == id);
        }
    }
}
