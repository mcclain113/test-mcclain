using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class AddressState
    {
        public AddressState()
        {
            Addresses = new HashSet<Address>();
        }

        public int StateID {get; set;}
        public string StateName {get; set;}
        public string StateAbbreviation {get; set;}

        public virtual ICollection<Address> Addresses { get; set; }
    }

}