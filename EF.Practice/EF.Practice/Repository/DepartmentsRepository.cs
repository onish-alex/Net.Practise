namespace EF.Practice.Repository
{
    using System.Linq;
    using EF.Practice.CodeFirst.Contexts;
    using EF.Practice.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DepartmentsRepository : EFRepository<Department>
    {
        public DepartmentsRepository(EmployeeDbContext db)
             : base(db)
        {
        }

        protected override IQueryable<Department> SelectQuery => this.db.Set<Department>()
                                                                          .Include(s => s.EmployeeDepartmentShifts)
                                                                            .ThenInclude(eds => eds.Department)
                                                                          .Include(s => s.EmployeeDepartmentShifts)
                                                                            .ThenInclude(eds => eds.Employee);

        public override Department GetById(int id)
        {
            return this.SelectQuery.SingleOrDefault(x => x.DepartmentId == id);
        }
    }
}
