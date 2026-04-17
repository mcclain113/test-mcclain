using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PatientGeneral
    {
        public string Mrn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string Gender { get; set; }
        public string LastFourSsn { get; set; }
        public string PreferredPhone { get; set; }
        public string ResidentAddress { get; set; }
        public string PrimaryInsuranceProvider { get; set; }
    }
}
