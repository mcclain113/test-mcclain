using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IS_Proj_HIT.DTOs;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Enum;

namespace IS_Proj_HIT.ViewModels.Encounters
{
    public class PhysicianAssessmentViewModel
    {
        public PhysicianAssessment PhysicianAssessment {get; set;}
        public Encounter Encounter {get; set;}
        [Required] public string PatientMrn { get; set; }

        public Physician EmergencyPhysician { get; set; }
        public Physician AdmittingPhysician { get; set; }
        public Physician AttendingPhysician { get; set; }
        public Physician PrimaryCarePhysician { get; set; }
        public Physician ReferringPhysician { get; set; }
        
        
        public List<Entities.ExamType> SystemExamTypes {get; set;}
        public List<BodySystemAssessment> BodySystemAssessments { get; set; }
        public List<PhysicianAssessmentAllergyDto> AssessmentAllergies { get; set; } = new();
        public List<PhysicianAssessmentAllergyDto> AssessmentMedications { get; set; } = new();
        public IEnumerable<Physician> Providers { get; set; }
        public IEnumerable<Physician> Physicians { get; set; }

        public PhysicianAssessmentViewModel()
        {
            PhysicianAssessment = new PhysicianAssessment();
            SystemExamTypes = new List<Entities.ExamType>();
            BodySystemAssessments = new List<BodySystemAssessment>();
            AssessmentAllergies = new List<DTOs.PhysicianAssessmentAllergyDto>();
            AssessmentMedications = new List<DTOs.PhysicianAssessmentAllergyDto>();
        }
        
        public PhysicianAssessmentViewModel(Encounter encounter)
        {
            PhysicianAssessment = new PhysicianAssessment();
            this.Encounter = encounter;
            SystemExamTypes = new List<Entities.ExamType>();
            BodySystemAssessments = new List<BodySystemAssessment>();
            AssessmentAllergies = new List<DTOs.PhysicianAssessmentAllergyDto>();
            AssessmentMedications = new List<DTOs.PhysicianAssessmentAllergyDto>();

            if (encounter.EncounterPhysicians != null)
            {
                foreach (EncounterPhysician ep in Encounter.EncounterPhysicians)
                {
                    if (ep.PhysicianRoleId == (int)PhysicianRoleType.AttendingPhysician)
                    {
                        AttendingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.EmergencyPhysician)
                    {
                        EmergencyPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.AdmittingPhysician)
                    {
                        AdmittingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.ReferringPhysician)
                    {
                        ReferringPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.PrimaryCare)
                    {
                        PrimaryCarePhysician = ep.Physician;
                    }
                }
            }
        }

        public PhysicianAssessmentViewModel (PhysicianAssessment physicianAssessment, Encounter encounter)
        {
            this.PhysicianAssessment = physicianAssessment;
            this.Encounter = encounter;
            AssessmentAllergies = new List<DTOs.PhysicianAssessmentAllergyDto>();
            AssessmentMedications = new List<DTOs.PhysicianAssessmentAllergyDto>();

            if (encounter.EncounterPhysicians != null)
            {
                foreach (EncounterPhysician ep in Encounter.EncounterPhysicians)
                {
                    if (ep.PhysicianRoleId == (int)PhysicianRoleType.AttendingPhysician)
                    {
                        AttendingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.EmergencyPhysician)
                    {
                        EmergencyPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.AdmittingPhysician)
                    {
                        AdmittingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.ReferringPhysician)
                    {
                        ReferringPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.PrimaryCare)
                    {
                        PrimaryCarePhysician = ep.Physician;
                    }
                }
            }
        }

        public PhysicianAssessmentViewModel (
            PhysicianAssessment physicianAssessment, 
            Encounter encounter, 
            List<Entities.ExamType> systemExamTypes, List<BodySystemAssessment> bodySystemAssessments,
            List<DTOs.PhysicianAssessmentAllergyDto> assessmentAllergies,
            List<DTOs.PhysicianAssessmentAllergyDto> assessmentMedications
            )
            
        {
            this.PhysicianAssessment = physicianAssessment;
            this.Encounter = encounter;
            this.SystemExamTypes = systemExamTypes;
            this.BodySystemAssessments = bodySystemAssessments;
            this.AssessmentAllergies = assessmentAllergies;
            this.AssessmentMedications = assessmentMedications;


            if (encounter.EncounterPhysicians != null)
            {
                foreach (EncounterPhysician ep in Encounter.EncounterPhysicians)
                {
                    if (ep.PhysicianRoleId == (int)PhysicianRoleType.AttendingPhysician)
                    {
                        AttendingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.EmergencyPhysician)
                    {
                        EmergencyPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.AdmittingPhysician)
                    {
                        AdmittingPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.ReferringPhysician)
                    {
                        ReferringPhysician = ep.Physician;
                    }
                    else if (ep.PhysicianRoleId == (int)PhysicianRoleType.PrimaryCare)
                    {
                        PrimaryCarePhysician = ep.Physician;
                    }
                }
            }
        }
        
    }

    
}