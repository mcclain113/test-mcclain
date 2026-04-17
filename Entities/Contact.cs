 using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities 
{
    public partial class Contact
    {
        public int ContactId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        #nullable enable
        public int? ShippingAddressId { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EmailAddress { get; set; }

        public virtual Address? ShippingAddress { get; set; }
        #nullable disable
        public virtual ICollection<Requester> Requesters { get; set; } = new List<Requester>();
    }
}


