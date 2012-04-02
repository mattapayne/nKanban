using System.ComponentModel.DataAnnotations;

namespace nKanban.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Your username is required.")]
        [Display(Name = "Username")]
        [StringLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters and no more than 20 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}