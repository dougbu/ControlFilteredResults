using System.ComponentModel.DataAnnotations;

namespace ControlFilteredResults.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
