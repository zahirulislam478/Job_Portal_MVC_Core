using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = default!;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Required, DataType(DataType.Password), Compare("Password"), Display(Name ="Confirm password")]
        public string ConfirmPassword { get; set; } = default!;
        public bool IsJobPoster { get; set; }
    }
}
