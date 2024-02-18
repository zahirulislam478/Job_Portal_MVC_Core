using JobPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace JobPortal.HostedServices
{
    public class IdentityDbInitializer
    {
        private readonly AppDbContext db;
        private readonly JobPortalDbContext jpdb;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public IdentityDbInitializer(AppDbContext db,JobPortalDbContext jpdb, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jpdb = jpdb;
            if(!this.db.Database.CanConnect())
            {
                this.db.Database.EnsureCreated();
            }
            if(!this.jpdb.Database.CanConnect())
            {
                this.jpdb.Database.EnsureCreated();
            }
        }
        public async Task SeedAsync()
        {


            await CreateRoleAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            await CreateRoleAsync(new IdentityRole { Name = "JobSeeker", NormalizedName = "JOOBSEEKER" });
            await CreateRoleAsync(new IdentityRole { Name = "JobPoster", NormalizedName = "JOPOSTER" });

            var hasher = new PasswordHasher<IdentityUser>();
            var user = new IdentityUser { UserName = "admin", NormalizedUserName = "ADMIN" };
            user.PasswordHash = hasher.HashPassword(user, "@Open1234");
            await CreateUserAsync(user, "Admin");


            user = new IdentityUser { UserName = "poster01", NormalizedUserName = "POSTER01" };
            user.PasswordHash = hasher.HashPassword(user, "@Open1234");
            await CreateUserAsync(user, "JobPoster");
            user = new IdentityUser { UserName = "user1", NormalizedUserName = "USER01" };
            user.PasswordHash = hasher.HashPassword(user, "@Open1234");
            await CreateUserAsync(user, "JobSeeker");

        }
        private async Task CreateRoleAsync(IdentityRole role)
        {
            var exits = await roleManager.RoleExistsAsync(role.Name ?? "");
            if (!exits)
                await roleManager.CreateAsync(role);
        }
        private async Task CreateUserAsync(IdentityUser user, string role)
        {
            var exists = await userManager.FindByNameAsync(user.UserName ?? "");
            if (exists == null)
            {
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, role);
            }

        }
    }
}
