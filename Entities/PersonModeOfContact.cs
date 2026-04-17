using System;

namespace IS_Proj_HIT.Entities;

public partial class PersonModeOfContact
{
    public long PersonModeOfContactId { get; set; }
    public long PersonContactDetailId { get; set; }
    public int ModeOfContactId { get; set; }
    public DateTime LastModified { get; set; }

    public virtual PersonContactDetail PersonContactDetail { get; set; }
    public virtual PreferredModeOfContact PreferredModeOfContact { get; set; }
}