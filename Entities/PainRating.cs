using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PainRating
    {
        public PainRating()
        {
            PcapainAssessments = new HashSet<PcapainAssessment>();
        }

        public int PainRatingId { get; set; }
        public int PainParameterId { get; set; }
        public byte Value { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual PainParameter PainParameter { get; set; }
        public virtual PainRatingImage PainRatingImage { get; set; }
        public virtual ICollection<PcapainAssessment> PcapainAssessments { get; set; }
    }
}
