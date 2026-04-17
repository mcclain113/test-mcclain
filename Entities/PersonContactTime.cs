using System;

namespace IS_Proj_HIT.Entities;

public partial class PersonContactTime
{
    public long PersonContactTimeId { get; set; }
    public long PersonContactDetailId { get; set; }
    public int ContactTimeId { get; set; }
    public DateTime LastModified { get; set; }

    public virtual PersonContactDetail PersonContactDetail { get; set; }
    public virtual PreferredContactTime PreferredContactTime { get; set; }
}