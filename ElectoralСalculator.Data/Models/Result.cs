using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralСalculator.Data.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public int VoterId { get; set; }
        public bool ValidVote { get; set; }
        public bool VotingRights { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}
