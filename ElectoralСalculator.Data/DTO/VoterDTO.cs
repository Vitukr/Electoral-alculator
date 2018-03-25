using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralСalculator.Data.DTO
{
    public class VoterDTO
    {
        public int Id { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Pesel { get; set; }        
        public string Password { get; set; }

        public string FullName 
        {
            get { return Forename + " " + Surname; }
        }
    }
}
