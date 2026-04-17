using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class UserSecurityQuestion
    {
        public int UserId { get; set; }
        public int SecurityQuestionId { get; set; }
        public string AnswerHash { get; set; }

        public virtual SecurityQuestion SecurityQuestion { get; set; }
        public virtual UserTable User { get; set; }
    }
}
