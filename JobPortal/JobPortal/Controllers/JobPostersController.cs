using JobPortal.Models;
using JobPortal.ViewModels.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace JobPortal.Controllers
{
    [Authorize(Roles ="JobPoster")]
    public class JobPostersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly JobPortalDbContext db;
        public JobPostersController(UserManager<IdentityUser> userManager, JobPortalDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public async Task<IActionResult> Index(int pg=1, string status = "open")
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            ViewBag.Status = status;
            if (user == null) { return BadRequest(); }
            var poster = await this.db.JobPosters.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if(poster == null) { return NotFound(); }
            if (status == "open")
            {
                var data = db.JobPosts                   
                    .Include(x=> x.JobPoster)
                    .Include(x=> x.JobSeekerJobPosts).ThenInclude(y=> y.JobSeeker)
                    .Where(x => !x.IsClosed && user.Id == x.JobPoster.UserId).ToList();
                return View(data.ToPagedList(pg, 5));
            }
            else if(status == "closed")
            {
                var data = db.JobPosts
                    .Include(x => x.JobPoster)
                    .Include(x => x.JobSeekerJobPosts).ThenInclude(y => y.JobSeeker)
                    .Where(x => x.IsClosed && user.Id == x.JobPoster.UserId).ToList();
                return View(data.ToPagedList(pg, 5));
            }
            else
            {
                var data = db.JobPosts
                    .Include(x => x.JobPoster)
                    .Include(x => x.JobSeekerJobPosts).ThenInclude(y => y.JobSeeker)
                 .Where(x => user.Id == x.JobPoster.UserId).ToList();
                return View(data.ToPagedList(pg, 5));
            }
            
        }
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null) { return BadRequest(); }
            var poster = await this.db.JobPosters.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if(poster == null) { return NotFound("Job Poster not found"); }
            return View(new JobPosterInputModel
            {
                JobPosterId= poster.JobPosterId,
                CompnayName=poster.CompnayName,
                CompnayAddress=poster.CompnayAddress,
                UserId=user.Id
            });
        }
        [HttpPost]
        public async Task<IActionResult> Profile(JobPoster model)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(JobPostInputModel post)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null) { return BadRequest(); }
            var poster = await this.db.JobPosters.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (ModelState.IsValid)
            {
                
                await db.JobPosts.AddAsync(new JobPost { JobPostName=post.JobPostName, Postion=post.Postion, Location=post.Location, Description=post.Description, JobPosterId=poster?.JobPosterId ?? 0});
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View();
        }
        public async Task<IActionResult> UpdateStatus(int id, bool status)
        {
            var jp = await db.JobPosts.FirstOrDefaultAsync(x=> x.JobPostId == id);
            if(jp == null)
            {
                return NotFound();
            }
            else
            {
                jp.IsClosed = status;
                await db.SaveChangesAsync();
                return Json(new { success = true });
            }
        }
    }
}
