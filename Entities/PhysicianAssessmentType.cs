using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PhysicianAssessmentType
    {
        public PhysicianAssessmentType()
        {
            PhysicianAssessments = new HashSet<PhysicianAssessment>();
        }

        public short PhysicianAssessmentTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual ICollection<PhysicianAssessment> PhysicianAssessments { get; set; }
    }
}
