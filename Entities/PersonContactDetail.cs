using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PersonContactDetail
    {
        public PersonContactDetail()
        {
            PersonModeOfContacts = new HashSet<PersonModeOfContact>();
            PersonContactTimes = new HashSet<PersonContactTime>();
        }
        public long PersonContactDetailId { get; set; }
        public int PersonId { get; set; }
        public DateTime LastModified { get; set; }

#nullable enable
        public string? CellPhone { get; set; }
        public string? HomePhone { get; set; }
        public string? WorkPhone { get; set; }
        public string? EmailAddress { get; set; }
        public int? MailingAddressId { get; set; }
        public int? ResidenceAddressId { get; set; }


        public virtual Address? MailingAddress { get; set; }
        public virtual Address? ResidenceAddress { get; set; }
#nullable disable
        public virtual Person Person { get; set; }
        
        public virtual ICollection<PersonModeOfContact> PersonModeOfContacts { get; set; }
        public virtual ICollection<PersonContactTime> PersonContactTimes { get; set; }
    }
}