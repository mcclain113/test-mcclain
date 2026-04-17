using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IS_Proj_HIT.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<SecurityQuestionAnswerVm> SecurityQuestionsAndAnswers { get; set; } = new();
    }
}
