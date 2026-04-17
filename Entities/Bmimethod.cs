using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Bmimethod
    {
        public Bmimethod()
        {
            Pcarecords = new HashSet<Pcarecord>();
        }

        public byte BmimethodId { get; set; }
        public string Name { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Pcarecord> Pcarecords { get; set; }
    }
}
