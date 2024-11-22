using CMCS.Data;  
using CMCS.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure database services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity services with custom User model and Role management
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Disable email confirmation for development
    options.Password.RequireDigit = true;          // Enforce password complexity
    options.Password.RequiredLength = 6;           // Minimum password length
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders(); // Generate tokens for password reset, etc.

// Configure cookie settings for authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";        // Redirect to login page if not authenticated
    options.LogoutPath = "/Identity/Account/Logout";      // Logout URL
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Redirect for unauthorized access
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);    // Session expiration
    options.SlidingExpiration = true;                    // Renew cookie on activity
});

builder.Services.AddValidatorsFromAssemblyContaining<ClaimValidator>();


// Add Razor Pages and MVC services
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");  // Show error page in production
    app.UseHsts();                           // Enforce HTTPS in production
}

app.UseHttpsRedirection();   // Redirect HTTP requests to HTTPS
app.UseStaticFiles();        // Serve static files (CSS, JS, images, etc.)

app.UseRouting();            // Enable endpoint routing

app.UseAuthentication();     // Enable authentication middleware
app.UseAuthorization();      // Enable authorization middleware

// Map Razor Pages for Identity
app.MapRazorPages();

// Set up default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the app
app.Run();
