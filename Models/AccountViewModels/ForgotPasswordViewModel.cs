using System.ComponentModel.DataAnnotations;

namespace ZRdotnetcore.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
