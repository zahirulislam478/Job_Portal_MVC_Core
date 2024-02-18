using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models
{
    public class JobPoster
    {
        public int JobPosterId { get; set; }
        [StringLength(50)]
        public string? CompnayName { get; set; }
        [StringLength(250)]
        public string? CompnayAddress { get; set; }
        [StringLength(450)]
        public string? UserId { get; set; } //locate his/her login

        public virtual ICollection<JobPost> JobPosts { get; set; }= new List<JobPost>();
    }
    public class JobPost
    {
        public int JobPostId { get; set; }
        [StringLength(40)]
        public string? JobPostName { get; set;}
        [StringLength(40)]
        public string? Postion { get; set; }
        [StringLength(40)]
        public string? Location { get; set;}
        [StringLength(250)]
        public string? Description { get; set; }

        public bool IsClosed { get; set; } = false;
        [Required, ForeignKey(nameof(JobPoster))] 
        public int JobPosterId { get; set; }
        public virtual JobPoster? JobPoster { get; set; } = default!;
        public virtual ICollection<JobSeekerJobPost> JobSeekerJobPosts { get; set; } = new List<JobSeekerJobPost>();

    }
    public class JobSeeker
    {
        public int JobSeekerId { get; set; }
        [StringLength(50)]
        public string? FullName { get; set; }
        [StringLength(50), DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [StringLength(20)]
        public string? Phone { get; set; }
        [StringLength(400)]
        public string? Description { get; set; }
        [StringLength(450)]
        public string? UserId { get; set; } //locate his/her login
        public virtual ICollection<JobSeekerJobPost> JobSeekerJobPosts { get; set; }= new List<JobSeekerJobPost>();
    }
    public class JobSeekerJobPost
    {
        [Required, ForeignKey(nameof(JobSeeker))]
        public int JobSeekerId { get; set; }
        [Required, ForeignKey(nameof(JobPost))]
        public int JobPostId { get; set; }

        public virtual JobSeeker? JobSeeker { get; set; } = default!;
        public virtual JobPost? JobPost { get; set; } = default!;
    }
    public class JobPortalDbContext : DbContext
    {
        public JobPortalDbContext(DbContextOptions<JobPortalDbContext> options):base(options) { }
        public DbSet<JobPoster> JobPosters { get; set; } = default!;
        public DbSet<JobPost> JobPosts { get; set; } = default!;
        public DbSet<JobSeeker> JobSeekers { get; set; } = default!;
        public DbSet<JobSeekerJobPost> JobSeekerJobPosts { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobSeekerJobPost>().HasKey(
                e => new { e.JobSeekerId, e.JobPostId });
        }
    }
}
