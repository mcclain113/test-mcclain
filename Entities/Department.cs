using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Department
    {
        public Department()
        {
            AdmitOrders = new HashSet<AdmitOrder>();
            ChargeDefinitions = new HashSet<ChargeDefinition>();
            Encounters = new HashSet<Encounter>();
        }

        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<AdmitOrder> AdmitOrders { get; set; }
        public virtual ICollection<ChargeDefinition> ChargeDefinitions {get; set;}
        public virtual ICollection<Encounter> Encounters { get; set; }
    }
}
