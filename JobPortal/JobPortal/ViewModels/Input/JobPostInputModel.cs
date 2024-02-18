using JobPortal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels.Input
{
    public class JobPostInputModel
    {
        public int JobPostId { get; set; }
        [Required, StringLength(40)]
        public string? JobPostName { get; set; }
        [Required, StringLength(40)]
        public string? Postion { get; set; }
        [Required, StringLength(40)]
        public string? Location { get; set; }
        [Required, StringLength(250)]
        public string? Description { get; set; }

        public bool IsClosed { get; set; } = false;
        [Required, ForeignKey(nameof(JobPoster))]
        public int JobPosterId { get; set; }
        public virtual JobPoster? JobPoster { get; set; } = default!;

    }
}
