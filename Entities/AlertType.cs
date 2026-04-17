using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class AlertType
    {
        public AlertType()
        {
            PatientAlerts = new HashSet<PatientAlert>();
        }

        public int AlertId { get; set; }

        [Required(ErrorMessage = "Please enter the Name for this Alert Type")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<PatientAlert> PatientAlerts { get; set; }
    }
}
