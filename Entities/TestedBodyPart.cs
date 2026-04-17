using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class TestedBodyPart
    {
        public long TestId { get; set; }
        public long PartId { get; set; }

        public virtual BodyPart Part { get; set; }
        public virtual Test Test { get; set; }
    }
}
