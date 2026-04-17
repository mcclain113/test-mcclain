using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class ProcedureReport
    {
        public ProcedureReport()
        {
            ProcedureReportAnestheticTypes = new HashSet<ProcedureReportAnestheticType>();
            ProcedureReportPhysicians = new HashSet<ProcedureReportPhysician>();
        }

        public long EncounterId { get; set; }
        public long ProcedureReportId { get; set; }
        public DateTime? ProcedureDate { get; set; }
        public string PreoperativeDiagonsis { get; set; }
        public string PostoperativeDiagnosis { get; set; }
        public string OperativeIndications { get; set; }
        public string ProcedurePerformed { get; set; }
        public string DescriptionOfProcedure { get; set; }
        public string Complications { get; set; }
        public decimal? EstiamtedBloodLoss { get; set; }
        public decimal? Drains { get; set; }
        public int? CoSignature { get; set; }
        public int AuthoringProvider { get; set; }
        public DateTime? WrittenDateTime { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int EditedBy { get; set; }

        public virtual Physician AuthoringProviderNavigation { get; set; }
        public virtual Physician CoSignatureNavigation { get; set; }
        public virtual Encounter Encounter { get; set; }
        public virtual ICollection<ProcedureReportAnestheticType> ProcedureReportAnestheticTypes { get; set; }
        public virtual ICollection<ProcedureReportPhysician> ProcedureReportPhysicians { get; set; }
    }
}
