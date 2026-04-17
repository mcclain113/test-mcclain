using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class TempRouteType
    {
        public TempRouteType()
        {
            Pcarecords = new HashSet<Pcarecord>();
        }

        public int TempRouteTypeId { get; set; }
        public string TempRouteTypeName { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Pcarecord> Pcarecords { get; set; }
    }
}
