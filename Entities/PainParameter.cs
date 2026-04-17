using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PainParameter
    {
        public PainParameter()
        {
            PainRatings = new HashSet<PainRating>();
            PcapainAssessments = new HashSet<PcapainAssessment>();
        }

        public int PainParameterId { get; set; }
        public int PainScaleTypeId { get; set; }
        public string ParameterName { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual PainScaleType PainScaleType { get; set; }
        public virtual ICollection<PainRating> PainRatings { get; set; }
        public virtual ICollection<PcapainAssessment> PcapainAssessments { get; set; }
    }
}
