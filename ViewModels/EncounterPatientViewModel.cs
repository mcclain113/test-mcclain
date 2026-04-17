using System;

namespace IS_Proj_HIT.ViewModels
{
    public class EncounterPatientViewModel
    {

        public EncounterPatientViewModel()
        { }
        public EncounterPatientViewModel(string Mrn, long EncounterId, DateTime AdmitDateTime,
            string FirstName, string LastName, string FacilityName, string DischargeDateTime, string roomNumber = null)
        {
            this.Mrn = Mrn;
            this.EncounterId = EncounterId;
            this.AdmitDateTime = AdmitDateTime;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.FacilityName = FacilityName;
            this.DischargeDateTime = DischargeDateTime;
            this.RoomNumber = roomNumber;
        }
        public string Mrn { get; set; }
        public long EncounterId { get; set; }
        public DateTime AdmitDateTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FacilityName { get; set; }
        public string DischargeDateTime { get; set; }
        public string RoomNumber { get; set; }
    }
}
