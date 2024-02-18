using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels.Input
{
    public class JobSeekerInputModel
    {
        public int JobSeekerId { get; set; }
        [Required, StringLength(50)]
        public string? FullName { get; set; }
        [Required, StringLength(50), DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required, StringLength(20)]
        public string? Phone { get; set; }
        [Required, StringLength(400)]
        public string? Description { get; set; }
        [Required, StringLength(450)]
        public string? UserId { get; set; } //locate his/her login
    }
}
