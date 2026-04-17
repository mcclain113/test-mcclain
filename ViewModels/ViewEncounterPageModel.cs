using System.Linq;
using IS_Proj_HIT.Entities;
using IS_Proj_HIT.Entities.Enum;

namespace IS_Proj_HIT.ViewModels
{
    public class ViewEncounterPageModel
    {
        public Encounter Encounter { get; set; }
        public Patient Patient { get; set; }
        public IQueryable<ProgressNote> ProgressNotes { get; set; }
        public PhysicianAssessment HistoryAndPhysical { get; set; }
        public Physician AuthoringProviderNavigation { get; set; }
        public int CoPhysicianId { get; set; }
        public int PhysicianId { get; set; }
        public Physician EmergencyPhysician { get; set; }
        public Physician AdmittingPhysician { get; set; }
        public Physician AttendingPhysician { get; set; }
        public Physician PrimaryCarePhysician { get; set; }
        public Physician ReferringPhysician { get; set; }


        // General Constructors
        public ViewEncounterPageModel()
        { }

         public ViewEncounterPageModel(Encounter encounter, Patient patient)
        {
            this.Encounter = encounter;
            this.Patient = patient;
            
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

        public ViewEncounterPageModel(Encounter encounter, Patient patient, PhysicianAssessment physicianAssessment)
        {
            this.Encounter = encounter;
            this.Patient = patient;
            this.HistoryAndPhysical = physicianAssessment;
            
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

        // Constructor for progress notes page
        public ViewEncounterPageModel(Encounter encounter, Patient patient, IQueryable<ProgressNote> progressNotes)
        {
            this.Encounter = encounter;
            this.Patient = patient;
            this.ProgressNotes = progressNotes;
        }
    }
}