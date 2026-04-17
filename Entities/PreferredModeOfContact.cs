using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PreferredModeOfContact
    {
        public PreferredModeOfContact()
        {
            PatientModeOfContacts = new HashSet<PatientModeOfContact>();
            PersonModeOfContacts = new HashSet<PersonModeOfContact>();
        }

        public int ModeOfContactId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
        
        public virtual ICollection<PatientModeOfContact> PatientModeOfContacts { get; set; }
        public virtual ICollection<PersonModeOfContact> PersonModeOfContacts { get; set; }
    }
}
