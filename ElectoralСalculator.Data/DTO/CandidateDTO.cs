﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralСalculator.Data.DTO
{
    public class CandidateDTO
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Party { get; set; }

        public virtual string FullName { get { return Name + " - " + Party; } }
    }
}
