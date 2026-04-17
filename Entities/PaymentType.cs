using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public class PaymentType
    {
        public byte PaymentTypeId { get; set; }

        public string PaymentType1 { get; set; } = null!;

        public virtual ICollection<DisclosurePayment> DisclosurePayments { get; set; } = new List<DisclosurePayment>();
    }
}
