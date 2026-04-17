using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class TestCategory
    {
        public TestCategory()
        {
            Tests = new HashSet<Test>();
        }

        public long TestCategoryId { get; set; }
        public string CategoryDescription { get; set; }
        public string TestCategoryName { get; set; }

        public virtual ICollection<Test> Tests { get; set; }
    }
}
