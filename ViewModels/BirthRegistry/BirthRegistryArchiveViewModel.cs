using System;

namespace IS_Proj_HIT.ViewModels.BirthRegistry
{
    public class BirthRegistryArchiveViewModel
    {
        public int BirthId { get; set; }
        public string MotherMrn { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherMiddleName { get; set; }
        public string MotherLastName { get; set; }
        public DateTime? MotherDob { get; set; }
        public string NewbornMrn { get; set; }
        public string NewbornFirstName { get; set; }
        public string NewbornMiddleName { get; set; }
        public string NewbornLastName { get; set; }
        public DateTime? NewbornDob { get; set; }
        public string FacilityName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string CertifierSignature { get; set; }
        public int BirthCount { get; set; }
        public bool IsComplete { get; set; }

        public string MotherName => FormatFullName(MotherFirstName, MotherMiddleName, MotherLastName);
        
        public string NewbornName
        {
            get
            {
                if (string.IsNullOrEmpty(NewbornFirstName) && string.IsNullOrEmpty(NewbornLastName))
                    return "Not Assigned";
                
                return FormatFullName(NewbornFirstName, NewbornMiddleName, NewbornLastName);
            }
        }

        private static string FormatFullName(string firstName, string middleName, string lastName)
        {
            return $"{firstName} {middleName} {lastName}".Replace("  ", " ").Trim();
        }
    }
}
