﻿#nullable disable

namespace EF.Practice.DatabaseFirst.Entities
{
    public partial class JobCandidate
    {
        public int JobCandidateId { get; set; }

        public int BusinessEntityId { get; set; }

        public string Resume { get; set; }

        public virtual Employee BusinessEntity { get; set; }
    }
}
