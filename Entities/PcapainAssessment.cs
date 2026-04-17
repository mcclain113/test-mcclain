using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PcapainAssessment
    {
        public long PainAssessmentId { get; set; }
        public int Pcaid { get; set; }
        public int PainParameterId { get; set; }
        public int PainRatingId { get; set; }
        public DateTime LastModified { get; set; }

        public virtual PainParameter PainParameter { get; set; }
        public virtual PainRating PainRating { get; set; }
        public virtual Pcarecord Pca { get; set; }
    }
}
