using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PainScaleType
    {
        public PainScaleType()
        {
            PainParameters = new HashSet<PainParameter>();
            Pcarecords = new HashSet<Pcarecord>();
        }

        public int PainScaleTypeId { get; set; }
        public string PainScaleTypeName { get; set; }
        public string UseDescription { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<PainParameter> PainParameters { get; set; }
        public virtual ICollection<Pcarecord> Pcarecords { get; set; }
    }
}
