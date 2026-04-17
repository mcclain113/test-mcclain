using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Language
    {
        public Language()
        {
            PatientLanguages = new HashSet<PatientLanguage>();
            PersonLanguages = new HashSet<PersonLanguage>();
        }

        public short LanguageId { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<PatientLanguage> PatientLanguages { get; set; }
        public virtual ICollection<PersonLanguage> PersonLanguages { get; set; }
    }
}
