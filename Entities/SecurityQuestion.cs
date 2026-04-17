using System;
using System.Collections.Generic;

#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class SecurityQuestion
    {
        public SecurityQuestion()
        {
            UserSecurityQuestions = new HashSet<UserSecurityQuestion>();
        }

        public int SecurityQuestionId { get; set; }
        public string QuestionText { get; set; }

        public virtual ICollection<UserSecurityQuestion> UserSecurityQuestions { get; set; }
    }
}
