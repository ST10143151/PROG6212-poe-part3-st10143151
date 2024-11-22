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

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CMCS.Models;

namespace CMCS.Controllers;

// Home controller
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    // Constructor Method
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    // Index Method
    public IActionResult Index()
    {
        return View();
    }
    // Privacy Method
    public IActionResult Privacy()
    {
        return View();
    }
    // Error Method
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
