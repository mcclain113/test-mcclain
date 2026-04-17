using System;

namespace IS_Proj_HIT.Services
{
    public class PatientSearchResult
    {
        public string Mrn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }

        public string Display => Dob.HasValue
            ? $"{Mrn} - {FirstName} {LastName} ({Dob.Value:MM-dd-yyyy})"
            : $"{Mrn} - {FirstName} {LastName}";
    }
}