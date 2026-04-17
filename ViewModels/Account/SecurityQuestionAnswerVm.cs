using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.ViewModels.Account
{
    public class SecurityQuestionAnswerVm
    {
        public int SecurityQuestionId { get; set; }
        public string QuestionText { get; set; }

        [Required]
        public string Answer { get; set; }

    }
}