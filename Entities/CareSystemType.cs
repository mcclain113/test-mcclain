using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class CareSystemType
    {
        public CareSystemType()
        {
            CareSystemParameters = new HashSet<CareSystemParameter>();
        }

        public short CareSystemTypeId { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<CareSystemParameter> CareSystemParameters { get; set; }
    }
}
