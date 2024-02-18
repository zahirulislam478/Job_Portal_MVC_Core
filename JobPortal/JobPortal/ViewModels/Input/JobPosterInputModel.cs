using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels.Input
{
    public class JobPosterInputModel
    {
        public int JobPosterId { get; set; }
        [Required, StringLength(50)]
        public string? CompnayName { get; set; }
        [Required, StringLength(250)]
        public string? CompnayAddress { get; set; }
        [Required, StringLength(450)]
        public string? UserId { get; set; } //locate his/her login
    }
}
