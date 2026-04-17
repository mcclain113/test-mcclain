using System;
using NuGet.Configuration;

namespace IS_Proj_HIT.Entities;

public partial class PatientModeOfContact
{
    public long PatientModeOfContactId { get; set; }
    public long PatientContactId { get; set; }
    public int ModeOfContactId { get; set; }
    public DateTime LastModified { get; set; }

    public virtual PatientContactDetail PatientContactDetail { get; set; }
    public virtual PreferredModeOfContact PreferredModeOfContact { get; set; }
}