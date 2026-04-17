using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class BodyPart
    {
        public BodyPart()
        {
            TestedBodyParts = new HashSet<TestedBodyPart>();
        }

        public long PartId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TestedBodyPart> TestedBodyParts { get; set; }
    }
}
