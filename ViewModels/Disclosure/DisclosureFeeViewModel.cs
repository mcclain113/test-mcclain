#nullable enable
namespace IS_Proj_HIT.ViewModels.Disclosure
{
    public class DisclosureFeeViewModel
    {
        public int DisclosureId { get; set; }
        public int DisclosureFeeTypeId { get; set; }
        public decimal FeeAmount { get; set; }
        public string FeeDescription { get; set; } = null!;
        public string? Comments { get; set; }
        public int ItemCount { get; set; }
        public decimal PerItemFee { get; set; }
        public decimal ItemTotal {get; set;}
    }
}