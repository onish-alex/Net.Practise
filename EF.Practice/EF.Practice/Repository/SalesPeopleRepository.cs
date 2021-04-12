namespace EF.Practice.Repository
{
    using System.Linq;
    using EF.Practice.CodeFirst.Contexts;
    using EF.Practice.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;

    public class SalesPeopleRepository : EFRepository<SalesPerson>
    {
        public SalesPeopleRepository(EmployeeDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<SalesPerson> SelectQuery => this.db.Set<SalesPerson>()
                                                                        .Include(sp => sp.Employee);

        public override SalesPerson GetById(int id)
        {
            return this.SelectQuery.SingleOrDefault(x => x.SalesPersonId == id);
        }
    }
}
