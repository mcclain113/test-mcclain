using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PcacommentType
    {
        public PcacommentType()
        {
            Pcacomments = new HashSet<Pcacomment>();
        }

        public int PcacommentTypeId { get; set; }
        public string PcacommentTypeName { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Pcacomment> Pcacomments { get; set; }
    }
}
