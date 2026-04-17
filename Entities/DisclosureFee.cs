namespace IS_Proj_HIT.Entities
{
    public class DisclosureFee
    {
        public int DisclosureId { get; set; }
        public Disclosure Disclosure { get; set; }

        public int DisclosureFeeTypeId { get; set; }
        public DisclosureFeeType DisclosureFeeType { get; set; }

        public int ItemCount { get; set; }
        public decimal PerItemFee { get; set; }
    }
}