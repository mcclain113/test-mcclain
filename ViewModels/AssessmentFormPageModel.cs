using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels
{
    public class AssessmentFormPageModel
    {
        public Pcarecord PcaRecord { get; set; }

        [Required] public string PatientMrn { get; set; }

        public string TempUnit { get; set; }
        public string VitalNote { get; set; }
        public string PainNote { get; set; }

        public List<CareSystemType> SecondarySystemTypes { get; set; }
        public Dictionary<int, CareSystemAssessment> Assessments { get; set; }

        public List<PainScaleType> PainScales { get; set; }
        public Dictionary<int, int?> PainRatings { get; set; }

        public AssessmentFormPageModel()
        {
            PcaRecord = new Pcarecord();

            SecondarySystemTypes = new List<CareSystemType>();
            Assessments = new Dictionary<int, CareSystemAssessment>();

            PainScales = new List<PainScaleType>();
            PainRatings = new Dictionary<int, int?>();
        }
    }
}