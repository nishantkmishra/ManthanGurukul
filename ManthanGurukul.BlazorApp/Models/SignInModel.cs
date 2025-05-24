using System.ComponentModel.DataAnnotations;

namespace ManthanGurukul.BlazorApp.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Mobile No is required")]
        [Range(1000000000, 9999999999, ErrorMessage = "Mobile No must be a 10-digit number")]
        public string MobileNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
