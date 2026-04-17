using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IS_Proj_HIT.ViewModels.Account
{
    public class SecurityQuestionsViewModel : IValidatableObject
    {
        public int CurrentUserId { get; set; }
        public string ReturnUrl { get; set; }
        public List<SelectListItem> Questions { get; set; }

        [Required]
        [Display(Name = "First Question")]
        public int SecurityQuestion1 { get; set; }

        [Required]
        [Display(Name = "Answer to First Question")]
        public string SecurityQuestion1Answer { get; set; }

        [Required]
        [Display(Name = "Second Question")]
        public int SecurityQuestion2 { get; set; }

        [Required]
        [Display(Name = "Answer to Second Question")]
        public string SecurityQuestion2Answer { get; set; }

        [Required]
        [Display(Name = "Third Question")]
        public int SecurityQuestion3 { get; set; }

        [Required]
        [Display(Name = "Answer to Third Question")]
        public string SecurityQuestion3Answer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var questionList = new List<int>
            {
                SecurityQuestion1,
                SecurityQuestion2,
                SecurityQuestion3
            };

            var distinctResults = questionList.Distinct().ToList();
            if (distinctResults.Count != 3)
            {
                yield return new ValidationResult("All questions must be unique!");
            }
        }
    }
}
