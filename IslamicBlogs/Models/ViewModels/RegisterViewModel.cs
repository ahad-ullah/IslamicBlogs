using System.ComponentModel.DataAnnotations;

namespace IslamicBlogs.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
