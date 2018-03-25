using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralСalculator.Data.Models
{
    public class Voter
    {
        public int Id { get; set; }
        [Required]
        public string Forename { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Pesel { get; set; }
        [Required]
        public string Password { get; set; }


        public virtual string FullName
        {
            get { return Forename + " " + Surname; }
        }
    }
}
