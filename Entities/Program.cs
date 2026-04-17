using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Program
    {
        public Program()
        {
            ProgramFacilities = new HashSet<ProgramFacility>();
            UserPrograms = new HashSet<UserProgram>();
        }

        public int ProgramId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<ProgramFacility> ProgramFacilities { get; set; }
        public virtual ICollection<UserProgram> UserPrograms { get; set; }
    }
}
