using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class ExamType
    {
        public ExamType()
        {
            BodySystemTypes = new HashSet<BodySystemType>();
        }

        public short ExamTypeCode { get; set; }
        public string ExamType1 { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual ICollection<BodySystemType> BodySystemTypes { get; set; }
    }
}
