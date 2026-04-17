using System.Collections.Generic;
using System.Linq;
using IS_Proj_HIT.Entities;

namespace  IS_Proj_HIT.ViewModels.PatientVm
{
    public class PatientListViewModel {
        public IEnumerable<Patient> Patients { get; set; }
        public IEnumerable<Patient> PatientsAllFacilities { get; set; }
        public IEnumerable<Patient> PatientsSessionFacility { get; set; }
        public string SessionFacilityName {get; set;}
    }
}
