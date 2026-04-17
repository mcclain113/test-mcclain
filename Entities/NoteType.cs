using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class NoteType
    {
        public NoteType()
        {
            ProgressNotes = new HashSet<ProgressNote>();
        }

        public int NoteTypeId { get; set; }
        
        [Required(ErrorMessage ="Please enter the Name")]
        public string NoteType1 { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual ICollection<ProgressNote> ProgressNotes { get; set; }
    }
}
