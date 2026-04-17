using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PreferredContactTime
    {
        public PreferredContactTime()
        {
            PatientContactTimes = new HashSet<PatientContactTime>();
        }

        public int ContactTimeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<PatientContactTime> PatientContactTimes { get; set; }
        public virtual ICollection<PersonContactTime> PersonContactTimes { get; set; }
    }
}
