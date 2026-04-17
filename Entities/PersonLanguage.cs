using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
        {
    public partial class PersonLanguage
    {
        public short PersonLanguageId { get; set; }
        public short LanguageId { get; set; }
        public int PersonId { get; set; }
        public byte IsPrimary { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Language Language { get; set; }
        public virtual Person Person { get; set; }
    }
}