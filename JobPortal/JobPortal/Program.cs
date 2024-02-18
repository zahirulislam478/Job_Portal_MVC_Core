using JobPortal.HostedServices;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
#region DbContexts
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("identity")));
builder.Services.AddDbContext<JobPortalDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("db")));
#endregion
#region HostedSerices
builder.Services.AddScoped<IdentityDbInitializer>();
builder.Services.AddHostedService<IdentityDbSeederHostedService>();
#endregion
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
        options => {
           
            options.SignIn.RequireConfirmedAccount = false;

            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        }
        )
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    //options.Cookie.Expiration 

    
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
   
    //options.ReturnUrlParameter=""
});
var app = builder.Build();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.Run();
