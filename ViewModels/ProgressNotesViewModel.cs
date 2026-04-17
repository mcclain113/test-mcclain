using System;
using IS_Proj_HIT.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.ViewModels
{
    public class ProgressNotesViewModel
    {
        
        public Encounter Encounter { get; set; }
        public Patient Patient { get; set; }
        public ProgressNote ProgressNote {get;set;}
        public int ProgressNoteId { get; set; }
        public long EncounterId { get; set; }
        public int NoteTypeId { get; set; }
        public DateTime? WrittenDate { get; set; }
        public string Note { get; set; }
        public int CoPhysicianId { get; set; }
        public int PhysicianId { get; set; }
        public int EditedBy { get; set; }
        public string AuthoringProviderSignature {get; set;}
        public DateTime? AuthoringProviderSignedDate {get; set;}
        public string CoSigningProviderSignature {get; set;}
        public DateTime? CoSigningProviderSignedDate {get; set;}

        
        // public bool CoSignRequired { get; set;}

        public IEnumerable<Physician> Providers { get; set; }
        
        public IEnumerable<Physician> Physicians { get; set; }

        // General Constructors
        public ProgressNotesViewModel()
        { }
        public ProgressNotesViewModel(long encounterId)
        {
            this.EncounterId = encounterId;
        }
        public ProgressNotesViewModel(Encounter encounter, Patient patient, ProgressNote progressNote)
        {
            this.Encounter = encounter;
            this.Patient = patient;
            this.ProgressNote = progressNote;
        }

        public ProgressNotesViewModel(Encounter encounter, Patient patient)
        {
            this.Encounter = encounter;
            this.Patient = patient;
        }
        
    }
}