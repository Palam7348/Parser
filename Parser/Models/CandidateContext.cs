using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Models
{
    class CandidateContext : DbContext
    {
        public CandidateContext() : base("DefaultConnection")
        {

        }

        public DbSet<Candidate> Candidates { get; set; }
    }
}
