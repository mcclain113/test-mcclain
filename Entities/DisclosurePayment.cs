using System;

namespace IS_Proj_HIT.Entities
{
    public class DisclosurePayment
    {
        public int DisclosurePaymentId { get; set; }

        public int DisclosureId { get; set; }

        public byte PaymentTypeId { get; set; }

        public decimal PaymentAmount { get; set; }

        public DateOnly PaymentDate { get; set; }

        public virtual Disclosure Disclosure { get; set; } = null!;

        public virtual PaymentType PaymentType { get; set; } = null!;
    }
}
