namespace IS_Proj_HIT.DTOs
{
    public class PhysicianAssessmentAllergyDto
    {
        public long? PhysicianAssessmentAllergyId { get; set; } // null for new
        public long? PhysicianAssessmentId { get; set; } // null or 0 for new assessment
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public string Type { get; set; } // "Allergy" or "Medication"
    }
}