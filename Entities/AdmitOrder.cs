using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class AdmitOrder
    {
        public long AdmitOrderId { get; set; }
        public short VisitTypeId { get; set; }
        public int DepartmentId { get; set; }
        public long OrderInfoId { get; set; }
        public string AdmittingDiagnosis { get; set; }
        public DateTime LastModified { get; set; }


        public virtual Department Department { get; set; }
        public virtual OrderInfo OrderInfo{ get; set; }
        public virtual VisitType VisitType { get; set; }
    }
}
