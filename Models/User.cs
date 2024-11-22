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

using Microsoft.AspNetCore.Identity;

namespace CMCS.Models
{
    
    public class User : IdentityUser
    {
       
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }
        public string? Office { get; set; }
        public string? OfficeHours { get; set; }
        public double HourlyRate {get; set;}
        public string? Profile { get; set; }
        public string? Image { get; set; }
        public string? Position { get; set; }
    }
}
