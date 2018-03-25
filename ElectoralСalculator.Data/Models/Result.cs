using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralСalculator.Data.Models
{
    public class Result
    {
        public int Id { get; set; }
        [Required]
        public int VoterId { get; set; }
        [Required]
        public bool ValidVote { get; set; }
        [Required]
        public bool VotingRights { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Party { get; set; }

        public virtual string NameParty { get { return Name + " - " + Party; } }
    }
}
