namespace IS_Proj_HIT.Entities;
using System;
using System.Collections.Generic;

public partial class GenderPronoun
{
    public GenderPronoun()
    {
        Patients = new HashSet<Patient>();
    }
    public byte GenderPronounId { get; set; }
    public string GenderPronouns { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastModified { get; set; }
    public virtual ICollection<Patient> Patients { get; set; }
    public virtual ICollection<Person> Persons { get; set; }
}