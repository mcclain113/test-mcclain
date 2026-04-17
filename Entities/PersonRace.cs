using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class PersonRace
    {
        public byte PersonRaceId { get; set; }
        public int PersonId { get; set; }
        public byte RaceId { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Person Person { get; set; }
        public virtual Race Race { get; set; }
    }
}