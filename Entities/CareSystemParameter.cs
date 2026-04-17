using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class CareSystemParameter
    {
        public CareSystemParameter()
        {
            CareSystemAssessments = new HashSet<CareSystemAssessment>();
        }

        public short CareSystemParameterId { get; set; }
        public string Name { get; set; }
        public string NormalLimitsDescription { get; set; }
        public short CareSystemTypeId { get; set; }
        public DateTime LastModified { get; set; }

        public virtual CareSystemType CareSystemType { get; set; }
        public virtual ICollection<CareSystemAssessment> CareSystemAssessments { get; set; }
    }
}
