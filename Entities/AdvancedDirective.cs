using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class AdvancedDirective
    {
        public AdvancedDirective()
        {
            PatientAdvancedDirectives = new HashSet<PatientAdvancedDirective>();
        }

        public short AdvancedDirectiveId { get; set; }

        [Required(ErrorMessage = "Please enter the Name for this Advanced Directive")]
        public string Name { get; set; }

        public virtual ICollection<PatientAdvancedDirective> PatientAdvancedDirectives { get; set; }
    }
}
