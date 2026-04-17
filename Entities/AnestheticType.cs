using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class AnestheticType
    {
        public AnestheticType()
        {
            ProcedureReportAnestheticTypes = new HashSet<ProcedureReportAnestheticType>();
        }

        public int AnestheticTypeId { get; set; }
        public string AnestheticType1 { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual ICollection<ProcedureReportAnestheticType> ProcedureReportAnestheticTypes { get; set; }
    }
}
