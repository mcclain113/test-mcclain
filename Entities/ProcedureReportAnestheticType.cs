using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class ProcedureReportAnestheticType
    {
        public long ProcedureReportId { get; set; }
        public int AnestheticTypeId { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual AnestheticType AnestheticType { get; set; }
        public virtual ProcedureReport ProcedureReport { get; set; }
    }
}
