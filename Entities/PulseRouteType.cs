using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PulseRouteType
    {
        public PulseRouteType()
        {
            Pcarecords = new HashSet<Pcarecord>();
        }

        public int PulseRouteTypeId { get; set; }
        public string PulseRouteTypeName { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Pcarecord> Pcarecords { get; set; }
    }
}
