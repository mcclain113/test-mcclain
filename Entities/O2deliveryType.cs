using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class O2deliveryType
    {
        public O2deliveryType()
        {
            Pcarecords = new HashSet<Pcarecord>();
        }

        public int O2deliveryTypeId { get; set; }
        public string O2deliveryTypeName { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Pcarecord> Pcarecords { get; set; }
    }
}
