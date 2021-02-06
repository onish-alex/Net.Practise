namespace EF.Practice.Repository
{
    using System.Linq;
    using EF.Practice.CodeFirst.Contexts;
    using EF.Practice.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ShiftsRepository : EFRepository<Shift>
    {
        public ShiftsRepository(EmployeeDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<Shift> SelectQuery => this.db.Set<Shift>()
                                                                  .Include(s => s.EmployeeDepartmentShifts)
                                                                    .ThenInclude(eds => eds.Department)
                                                                  .Include(s => s.EmployeeDepartmentShifts)
                                                                    .ThenInclude(eds => eds.Employee);

        public override Shift GetById(int id)
        {
            return this.SelectQuery.SingleOrDefault(x => x.ShiftId == id);
        }
    }
}
