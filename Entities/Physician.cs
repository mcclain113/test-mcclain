using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Physician
    {
        public Physician()
        {
            PatientMedicationLists = new HashSet<PatientMedicationList>();
            EmarAdministeredByNavigations = new HashSet<Emar>();
            EmarAssignedAdministratorNavigations = new HashSet<Emar>();
            DischargeAuthoringProviders = new HashSet<Encounter>();
            DischargeCoSigningProviders = new HashSet<Encounter>();
            EncounterPhysiciansNavigation = new HashSet<EncounterPhysician>();
            OrderInfoAuthors = new HashSet<OrderInfo>();
            OrderInfoCoProviders = new HashSet<OrderInfo>();
            OrderInfoOrderingProviderNavigations = new HashSet<OrderInfo>();
            OrderCompletedByProvider = new HashSet<OrderInfo>();
            PhysicianAssessmentAuthoringProviderNavigations = new HashSet<PhysicianAssessment>();
            PhysicianAssessmentCoSignatureNavigations = new HashSet<PhysicianAssessment>();
            PhysicianAssessmentReferringProviderNavigations = new HashSet<PhysicianAssessment>();
            ProcedureReportAuthoringProviderNavigations = new HashSet<ProcedureReport>();
            ProcedureReportCoSignatureNavigations = new HashSet<ProcedureReport>();
            ProcedureReportPhysicians = new HashSet<ProcedureReportPhysician>();
            ProgressNoteCoPhysicians = new HashSet<ProgressNote>();
            ProgressNotePhysicians = new HashSet<ProgressNote>();
        }

        public int PhysicianId { get; set; }

        [Required(ErrorMessage = "Please enter the Given (First) Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the Family Name")]
        public string LastName { get; set; }
        public string Credentials { get; set; }
        public string License { get; set; }
        public int? AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please select a Specialty")]
        public int? SpecialtyId { get; set; }

        [Required(ErrorMessage = "Please select a Provider Type")]
        public int? ProviderTypeId { get; set; }

        [Required(ErrorMessage = "Please select a Status")]
        public byte? ProviderStatusId { get; set; }
        public DateTime LastModified { get; set; }
        public short? Pin { get; set; }

        public virtual Address Address { get; set; }
        public virtual ProviderType ProviderType { get; set; }
        public virtual ProviderStatus ProviderStatus { get; set; }
        public virtual Specialty Specialty { get; set; }

        //medication list collection
        public virtual ICollection<PatientMedicationList> PatientMedicationLists { get; set; }
        public virtual ICollection<Emar> EmarAdministeredByNavigations { get; set; }
        public virtual ICollection<Emar> EmarAssignedAdministratorNavigations { get; set; }

        public virtual ICollection<Encounter> DischargeAuthoringProviders { get; set; }
        public virtual ICollection<Encounter> DischargeCoSigningProviders { get; set; }

        public virtual ICollection<EncounterPhysician> EncounterPhysiciansNavigation { get; set; }
        public virtual ICollection<OrderInfo> OrderInfoAuthors { get; set; }
        public virtual ICollection<OrderInfo> OrderInfoCoProviders { get; set; }
        public virtual ICollection<OrderInfo> OrderCompletedByProvider { get; set; }
        public virtual ICollection<OrderInfo> OrderInfoOrderingProviderNavigations { get; set; }
        public virtual ICollection<PhysicianAssessment> PhysicianAssessmentAuthoringProviderNavigations { get; set; }
        public virtual ICollection<PhysicianAssessment> PhysicianAssessmentCoSignatureNavigations { get; set; }
        public virtual ICollection<PhysicianAssessment> PhysicianAssessmentReferringProviderNavigations { get; set; }
        public virtual ICollection<ProcedureReport> ProcedureReportAuthoringProviderNavigations { get; set; }
        public virtual ICollection<ProcedureReport> ProcedureReportCoSignatureNavigations { get; set; }
        public virtual ICollection<ProcedureReportPhysician> ProcedureReportPhysicians { get; set; }
        public virtual ICollection<ProgressNote> ProgressNoteCoPhysicians { get; set; }
        public virtual ICollection<ProgressNote> ProgressNotePhysicians { get; set; }

        public virtual ICollection<Birth> BirthCertifierOfBirths { get; set; } = [];
        public virtual ICollection<Birth> BirthDeliveringAttendants{ get; set; } = [];
    }
}
