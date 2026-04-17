using System;
using System.Collections.Generic;

namespace IS_Proj_HIT.Entities
{
    public partial class MedicationDeliveryRoute
    {
        public short DeliveryRouteId {get; set;}
        public string DeliveryRouteName {get; set;}
        public string Description {get; set;}
        public bool IsActive {get; set;}
        public DateTime ModifiedDate {get; set;}

       public virtual ICollection<Medication> Medications { get; set; }   
        
    }
}