#nullable enable
using System.Collections.Generic;
using System;

namespace IS_Proj_HIT.Entities
{
    public class DisclosureFeeType
    {
        public int DisclosureFeeTypeId { get; set; }
        public int SortOrder { get; set; }

        public decimal FeeAmount { get; set; }

        public string FeeDescription { get; set; } = null!;

        public string? Comments { get; set; }

        public DateOnly EffectiveDate { get; set; }

        public DateOnly? ExpirationDate { get; set; }

        public virtual ICollection<Disclosure> Disclosures { get; set; } = new List<Disclosure>();
        public ICollection<DisclosureFee> DisclosureFees { get; set; } = null!;
    }
}
