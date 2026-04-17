using System;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class ExamBodySystem
    {
        public short ExamTypeCode { get; set; }
        public short CareSystemTypeId { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual BodySystemType CareSystemType { get; set; }
        public virtual ExamType ExamTypeCodeNavigation { get; set; }
    }
}
