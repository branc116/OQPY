using System.ComponentModel.DataAnnotations;

namespace OQPYManager.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}