using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Emar
    {
        public long MedicationAdministrationId { get; set; }
        public long ApprovedMedicineId { get; set; }
        public DateTime AdministeredAttempt { get; set; }
        public int AdministrationStatusId { get; set; }
        public int? AssignedAdministrator { get; set; }
        public int AdministeredBy { get; set; }
        public decimal? AdministrationCost { get; set; }

        public virtual Physician AdministeredByNavigation { get; set; }
        public virtual Physician AssignedAdministratorNavigation { get; set; }
    }
}