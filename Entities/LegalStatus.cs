namespace IS_Proj_HIT.Entities;
using System;
using System.Collections.Generic;

public partial class LegalStatus
{
    public LegalStatus()
    {
        Patients = new HashSet<Patient>();
    }
    public byte LegalStatusId { get; set; }
    public string LegalStatusName { get; set; }
    public bool RequiresLegalGuardian { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastModified { get; set; }
    public virtual ICollection<Patient> Patients { get; set; }
    
}