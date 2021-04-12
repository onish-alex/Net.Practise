namespace EF.Practice.Repository
{
    using System.Linq;
    using EF.Practice.CodeFirst.Contexts;
    using EF.Practice.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;

    public class EmployeePayHistoryRepository : EFRepository<EmployeePayHistory>
    {
        public EmployeePayHistoryRepository(EmployeeDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<EmployeePayHistory> SelectQuery => this.db.Set<EmployeePayHistory>()
                                                                                .Include(eph => eph.Employee);

        public override EmployeePayHistory GetById(int id)
        {
            return this.SelectQuery.SingleOrDefault(x => x.EmployeePayHistoryId == id);
        }
    }
}
