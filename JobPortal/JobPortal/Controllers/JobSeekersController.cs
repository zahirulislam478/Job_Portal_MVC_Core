using JobPortal.Models;
using JobPortal.ViewModels.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace JobPortal.Controllers
{
    [Authorize(Roles ="JobSeeker")]
    public class JobSeekersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly JobPortalDbContext db;
        public JobSeekersController(UserManager<IdentityUser> userManager, JobPortalDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null) { return BadRequest(); }
            var jobseeker = await db.JobSeekers.FirstOrDefaultAsync(db => db.UserId == user.Id);
            var data = await db.JobSeekerJobPosts.Include(x=> x.JobPost).ThenInclude(y=> y.JobPoster).ToListAsync();
            return View(await data.OrderByDescending(x=>x.JobPostId).ToPagedListAsync(pg, 5));
        }
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null) { return BadRequest(); }
            var seeker = await this.db.JobSeekers.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (seeker == null) { return NotFound("Job Poster not found"); }
            return View(new JobSeekerInputModel {  
                JobSeekerId=seeker.JobSeekerId,
                FullName=seeker.FullName,
                Email=seeker.Email,
                Phone=seeker.Phone,
                Description=seeker.Description,
                UserId=user.Id
            });
        }
        [HttpPost]
        public async Task<IActionResult> Profile(JobSeeker model)
        {
            if (ModelState.IsValid)
            {

                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> Apply(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null) { return BadRequest(); }
            var jobseeker = await db.JobSeekers.FirstOrDefaultAsync(db => db.UserId == user.Id);
            if(jobseeker == null) { return NotFound(); }
            if(await db.JobSeekerJobPosts.AnyAsync(x=> x.JobPostId==id && x.JobSeekerId== jobseeker.JobSeekerId))
            {
                return RedirectToAction("Index");
            }
            await db.JobSeekerJobPosts.AddAsync(new JobSeekerJobPost { JobPostId = id, JobSeekerId = jobseeker.JobSeekerId });
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
