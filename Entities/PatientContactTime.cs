using System;

namespace IS_Proj_HIT.Entities;

public partial class PatientContactTime
{
    public long PatientContactTimeId { get; set; }
    public long PatientContactId { get; set; }
    public int ContactTimeId { get; set; }
    public DateTime LastModified { get; set; }

    public virtual PatientContactDetail PatientContactDetail { get; set; }
    public virtual PreferredContactTime PreferredContactTime { get; set; }
}