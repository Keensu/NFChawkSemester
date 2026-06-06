using System.ComponentModel.DataAnnotations;

namespace NFChawk.Models.ViewModels
{
    public class LogInViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
