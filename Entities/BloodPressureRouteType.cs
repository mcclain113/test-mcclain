using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class BloodPressureRouteType
    {
        public BloodPressureRouteType()
        {
            Pcarecords = new HashSet<Pcarecord>();
        }

        public byte BloodPressureRouteTypeId { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Pcarecord> Pcarecords { get; set; }
    }
}
