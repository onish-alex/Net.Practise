namespace EF.Practice
{
    using EF.Practice.CodeFirst.Contexts;
    using EF.Practice.Repository;

    public class Program
    {
        public static void Main(string[] args)
        {
            var db = new EmployeeDbContext();

            var jc = new JobCandidateRepository(db);
            var sp = new SalesPeopleRepository(db);
            var shift = new ShiftsRepository(db);
            var dep = new DepartmentsRepository(db);
            var history = new EmployeePayHistoryRepository(db);
            var emp = new EmployeeRepository(db);

            var jc1 = jc.GetAll();
        }
    }
}
