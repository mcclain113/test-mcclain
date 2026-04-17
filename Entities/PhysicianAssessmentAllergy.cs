using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PhysicianAssessmentAllergy
    {
        public long PhysicianAssessmentAllergyId { get; set; }
        public long PhysicianAssessmentId { get; set; }
        public string Description {get;set;}
        public string Type {get; set;}
        public int SortOrder {get; set;}

        public virtual PhysicianAssessment PhysicianAssessment { get; set; }
    }
}
