#nullable disable

namespace IS_Proj_HIT.Entities
{
    public partial class UserQuestion
    {
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public string AnswerHash { get; set; }

        public virtual SecurityQuestion Question { get; set; }
        public virtual UserTable User { get; set; }
    }
}
