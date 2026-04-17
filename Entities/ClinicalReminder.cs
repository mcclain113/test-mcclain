using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class ClinicalReminder
    {
        public ClinicalReminder()
        {
            PatientClinicalReminders = new HashSet<PatientClinicalReminder>();
        }

        public short ClinicalReminderId { get; set; }
        [Required(ErrorMessage = "Please enter the Name for this Reminder")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select Yes or No")]
        public bool IsActive { get; set; } 

        public virtual ICollection<PatientClinicalReminder> PatientClinicalReminders { get; set; }
    }
}
