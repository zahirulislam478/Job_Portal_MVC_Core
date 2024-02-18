using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
namespace JobPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly JobPortalDbContext db;
        public HomeController(JobPortalDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index(int pg=1)
        {
            var data = this.db.JobPosts.Where(x=> !x.IsClosed).OrderBy(X => X.JobPostId).ToPagedList(pg, 5);
            return View(data);
        }
    }
}
