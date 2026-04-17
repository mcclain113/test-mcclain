using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.Entities.Enum
{
    public enum BodySystemAssessmentStatuses
    {
        NotAssessed,
        WithinNormalLimits,
        [Required] 
        Abnormal,
    }
}