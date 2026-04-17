using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class CareSystemAssessment
    {
        public int CareSystemAssessmentId { get; set; }
        public int Pcaid { get; set; }
        public short CareSystemParameterId { get; set; }
        public bool? IsWithinNormalLimits { get; set; }
        public string Comment { get; set; }
        public DateTime LastModified { get; set; }

        public virtual CareSystemParameter CareSystemParameter { get; set; }
        public virtual Pcarecord Pca { get; set; }
    }
}
