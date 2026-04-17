#nullable enable
using System.Collections.Generic;
using System;

namespace IS_Proj_HIT.Entities
{
    public class Disclosure
    {
        public int DisclosureId { get; set; }

        public int? RequestId { get; set; }

        public int? RequesterId { get; set; }

        public DateOnly? DisclosureDate { get; set; }

        public int? InvoiceNumber { get; set; }

        public DateOnly? InvoiceDate { get; set; }
        public decimal? InvoiceTotal { get; set; }

        public bool? IsPaymentRequired { get; set; }

        public string? Comments { get; set; }



        public virtual Request? Request { get; set; }

        public virtual Requester? Requester { get; set; }
        
        public virtual ICollection<DisclosurePayment> DisclosurePayments { get; set; } = new List<DisclosurePayment>();

        public virtual ICollection<DisclosureFeeType> DisclosureFeeTypes { get; set; } = new List<DisclosureFeeType>();
        public ICollection<DisclosureFee> DisclosureFees { get; set; } = null!;

    }
}
