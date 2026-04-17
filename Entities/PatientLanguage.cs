using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
        {
    public partial class PatientLanguage
    {
        public short LanguageId { get; set; }
        public string Mrn { get; set; }
        public byte IsPrimary { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Language Language { get; set; }
        public virtual Patient MrnNavigation { get; set; }
    }
}
