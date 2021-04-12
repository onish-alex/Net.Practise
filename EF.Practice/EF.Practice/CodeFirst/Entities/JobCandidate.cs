namespace EF.Practice.CodeFirst.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("JobCandidates", Schema = "HumanResources")]
    public class JobCandidate
    {
        public int JobCandidateId { get; set; }

        public int BusinessEntityId { get; set; }

        public Employee Employee { get; set; }

        public string Resume { get; set; }
    }
}
