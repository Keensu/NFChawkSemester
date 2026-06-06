using System.ComponentModel.DataAnnotations;

namespace NFChawk.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
