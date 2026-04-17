using System;
using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels
{
    public class EncounterFormViewModel
    {
        public Encounter encounter {get; set;}
        public Patient patient {get; set;}
        public string Mrn { get; set; }
        public long? EncounterRestrictionId { get; set; }
        public long EncounterId { get; set; }
        public int FacilityId { get; set; }
        public byte PatientCurrentAge { get; set; }
        public DateTime AdmitDateTime { get; set; }
        public string ChiefComplaint { get; set; }
        public int? EncounterTypeId { get; set; }
        public int PlaceOfServiceId { get; set; }
        public int AdmitTypeId { get; set; }
        public string RoomNumber { get; set; }
        public bool FacilityRegistryOptInOut { get; set; }
        public int? DepartmentId { get; set; }
        public int PointOfOriginId { get; set; }
        public int? DischargeDisposition { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime? DischargeDateTime { get; set; }
        public string DischargeComment { get; set; }
        public int? PhysicianId { get; set; }
        public string AdmittingDiagnosis { get; set; }
        public string HistoryOfPresentIllness { get; set; }
        public string SignificantFindings { get; set; }
        public string MedicationsOnDischarge { get; set; }
        public string DischargeDietInstructions { get; set; }
        public int? CoSignature { get; set; }
        public DateTime? WrittenDateTime { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? EditedBy { get; set; }
        public string DischargeDispositionNote { get; set; }
        public int? AuthoringProvider { get; set; }
        public string DischargeHospitalCourse { get; set; }
        public string DischargeDiagnosis { get; set; }


        public int? emergencyProviderId {get; set;}
        public int? admittingProviderId {get; set;}
        public int? attendingProviderId {get; set;}
        public int? primaryProviderId {get; set;}
        public int? referringProviderId {get; set;}

        
    }
}