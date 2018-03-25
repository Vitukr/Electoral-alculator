using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralСalculator.Data.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Party { get; set; }

        public virtual string FullName
        {
            get { return Name + " " + Party; }
        }
    }
}
