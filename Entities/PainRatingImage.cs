using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PainRatingImage
    {
        public int PainRatingId { get; set; }
        public byte[] Image { get; set; }
        public DateTime LastModified { get; set; }

        public virtual PainRating PainRating { get; set; }
    }
}
