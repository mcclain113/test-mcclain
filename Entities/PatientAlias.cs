using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PatientAlias
    {

        public int PatientAliasID {get; set;}
        public string PatientMRN {get; set;}
        public string AliasFirstName {get; set;}
        public string AliasMiddleName {get; set;}
        public string AliasLastName {get; set;}
        public byte? AliasPriority {get; set;}
        public DateTime LastModified {get; set;}
        public int? EditedBy {get; set;}

        public virtual Patient Patient { get; set; }
    
    }
}