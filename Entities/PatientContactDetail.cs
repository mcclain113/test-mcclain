using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PatientContactDetail
    {
        public long PatientContactId { get; set; }
        public string Mrn { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string EmailAddress { get; set; }
        public int? MailingAddressId { get; set; }
        public int? ResidenceAddressId { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Address MailingAddress { get; set; }
        public virtual Patient MrnNavigation { get; set; }
        public virtual Address ResidenceAddress { get; set; }
        public virtual ICollection<PatientModeOfContact> PatientModeOfContacts { get; set; }
        public virtual ICollection<PatientContactTime> PatientContactTimes { get; set; }
    }
}
