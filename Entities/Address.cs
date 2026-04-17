using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Address
    {
        public Address()
        {
            Facilities = new HashSet<Facility>();
            PatientContactDetailMailingAddresses = new HashSet<PatientContactDetail>();
            PatientContactDetailResidenceAddresses = new HashSet<PatientContactDetail>();
            MailingAddresses = new HashSet<PersonContactDetail>();
            ResidenceAddresses = new HashSet<PersonContactDetail>();
            PatientEmergencyContacts = new HashSet<PatientEmergencyContact>();
            Physicians = new HashSet<Physician>();
            Employments = new HashSet<Employment>();
            ContactShippingAddresses = new HashSet<Contact>();
        }

        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public int? AddressStateID { get; set; }
        public int CountryId { get; set; }
        public DateTime LastModified { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public virtual AddressState AddressState { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Facility> Facilities { get; set; }
        public virtual ICollection<PatientContactDetail> PatientContactDetailMailingAddresses { get; set; }
        public virtual ICollection<PatientContactDetail> PatientContactDetailResidenceAddresses { get; set; }
        public virtual ICollection<PersonContactDetail> MailingAddresses { get; set; }
        public virtual ICollection<PersonContactDetail> ResidenceAddresses { get; set; }
        public virtual ICollection<PatientEmergencyContact> PatientEmergencyContacts { get; set; }
        public virtual ICollection<Physician> Physicians { get; set; }
        public virtual ICollection<Requester> Requesters { get; set; } = new List<Requester>();
        public virtual ICollection<Employment> Employments { get; set; }
        public virtual ICollection<Contact> ContactShippingAddresses { get; set; }
        public virtual ICollection<Birth> Births { get; set; } = [];
    }
}
