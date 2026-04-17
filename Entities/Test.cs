using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class Test
    {
        public Test()
        {
            TestedBodyParts = new HashSet<TestedBodyPart>();
        }

        public long TestId { get; set; }
        public long TestCategoryId { get; set; }
        public string TestName { get; set; }
        public string Description { get; set; }

        public virtual TestCategory TestCategory { get; set; }
        public virtual ICollection<TestedBodyPart> TestedBodyParts { get; set; }
    }
}
