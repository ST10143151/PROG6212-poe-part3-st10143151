// Code Attribution:

//Allen, D. and Foster, A., 2022. Pro ASP.NET Core Identity: Under the Hood of Authentication and Authorization 
//for ASP.NET Core Applications. New York: Apress.
//Esposito, D., 2021. Modern Web Development with ASP.NET Core 5: An end-to-end guide to becoming a professional 
//full-stack web developer. Birmingham: Packt Publishing.
//Johnson, M., 2019. Improving efficiency through digital claims management. International Journal of Systems and 
//Applications, 7(4), pp.102-115.
//Microsoft, 2023. ASP.NET Core MVC Overview. Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0 [Accessed 18 October 2024].
//Microsoft, 2024. Entity Framework Core Documentation. Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/ef/core/ [Accessed 18 November 2024].
//Microsoft, 2024. Introduction to Identity on ASP.NET Core. Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-8.0 [Accessed 18 November 2024].
//Microsoft, 2024. Configure ASP.NET Core Identity. Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-9.0 [Accessed 18 November 2024].
//Microsoft, 2024. EF Core Tools Reference (.NET CLI). Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/ef/core/cli/dotnet/ [Accessed 18 November 2024].
//Troelsen, A. and Japikse, P., 2021. Pro C# 9 with .NET 5: Foundational Principles and Practices in Programming.
//Full code attribution in Readme.md
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
