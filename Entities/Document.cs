using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public partial class Document
    {
        public int DocumentID { get; set; }
        public string Mrn { get; set; } // ALWAYS associate to the Patient
        public string FileName { get; set; } // DocumentType-MRN-ENCOUNTER-YYYMMDDTTTT-sequence
        public byte DocumentTypeID { get; set; } // Patient photo, Lab Results, Radiology Results, etc
        public string FileType { get; set; } // pdf, jpeg, etc.
        public byte[] DocumentContent { get; set; } //the actual document, EF maps to varbinary(max)
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }

#nullable enable
        public string? DocumentDescription { get; set; } // like 'Lab report from..."
        public string? Notes { get; set; } // more detailed info
        public long? EncounterId { get; set; }  // associate to an Encounter - only if it is Encounter level
#nullable disable

        public Document() {}

        public virtual DocumentType DocumentType { get; set; }  // single-entity nav
        public virtual Patient Patient {get; set;}
        public virtual Encounter Encounter {get; set;}
    }
}