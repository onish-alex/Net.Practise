namespace EF.Practice.Repository
{
    using System.Linq;
    using EF.Practice.CodeFirst.Contexts;
    using EF.Practice.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;

    public class EmployeeRepository : EFRepository<Employee>
    {
        public EmployeeRepository(EmployeeDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<Employee> SelectQuery => this.db.Set<Employee>()
                                                                        .Include(e => e.SalesPeople)
                                                                        .Include(e => e.JobCandidates)
                                                                        .Include(e => e.Person)
                                                                        .Include(e => e.EmployeeDepartmentShifts)
                                                                            .ThenInclude(eds => eds.Shift)
                                                                        .Include(e => e.EmployeeDepartmentShifts)
                                                                            .ThenInclude(eds => eds.Department);

        public override Employee GetById(int id)
        {
            return this.SelectQuery.SingleOrDefault(x => x.BusinessEntityId == id);
        }
    }
}
