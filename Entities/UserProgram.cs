using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class UserProgram
    {
        public int UserId { get; set; }
        public int ProgramId { get; set; }

        public virtual Program Program { get; set; }
        public virtual UserTable User { get; set; }
    }
}
