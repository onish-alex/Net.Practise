namespace EF.Practice.Repository
{
    using System.Linq;
    using EF.Practice.CodeFirst.Contexts;
    using EF.Practice.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;

    public class JobCandidateRepository : EFRepository<JobCandidate>
    {
        public JobCandidateRepository(EmployeeDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<JobCandidate> SelectQuery => this.db.Set<JobCandidate>()
                                                                          .Include(jc => jc.Employee);

        public override JobCandidate GetById(int id)
        {
            return this.SelectQuery.SingleOrDefault(x => x.JobCandidateId == id);
        }
    }
}
