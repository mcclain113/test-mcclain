using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class ProviderStatus
    {
        public ProviderStatus()
        {
            Physicians = new HashSet<Physician>();
        }

        public byte ProviderStatusId { get; set; }
        public string Status { get; set; }
        public DateTime LastModified { get; set; }
        public virtual ICollection<Physician> Physicians { get; set; }
    }
}
