# PROG6212-poe-part3-st10143151
 



# Contract Monthly Claim System (CMCS)

## Overview
The Contract Monthly Claim System (CMCS) is a web-based application designed to streamline the submission and approval process of monthly claims for Independent Contractor (IC) lecturers. Built with C# and ASP.NET Core MVC, the system integrates user-friendly interfaces with robust functionality to enhance efficiency, accuracy, and transparency in claim management.

## Features

### Lecturer View
- Automated Claim Submission: Submit claims with auto-calculated payment based on hours worked and hourly rates.
- Document Upload: Securely upload and attach supporting documents to claims.
- Real-Time Status Tracking: Track claim statuses (`Pending`, `Approved`, or `Rejected`) via a transparent and intuitive dashboard.

### Coordinator and Manager View
- Claim Verification and Approval: Review, approve, or reject claims efficiently.
- Workflow Automation: Streamlined workflows to enhance processing and reduce errors.

### HR View
- Administrative Automation: Automates claim processing and lecturer data management tasks.
- Report Generation: Produces summaries of approved claims for payment processing using integrated reporting tools.

## Key Technologies
- Frontend: ASP.NET Core MVC, Razor Views
- Backend: C#, ASP.NET Core, Entity Framework Core
- Database: SQL Server
- Automation: ASP.NET Identity for authentication and Entity Framework for data manipulation
- Version Control: Git and GitHub

## Installation

### Prerequisites
- Visual Studio 2022 or later
- .NET 8 SDK
- SQL Server (LocalDB or equivalent)
- Git

### Steps
1. Clone this repository:
   ```bash
   git clone <repository-url>
   cd CMCS
   ```
2. Open the solution file (`CMCS.sln`) in Visual Studio.
3. Configure the database connection string in `appsettings.json`:
   ```json
   {
       "ConnectionStrings": {
           "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CMCS;Trusted_Connection=True;"
       }
   }
   ```
4. Apply migrations and seed the database:
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet run
   ```
6. Open the application in your browser at `http://localhost:5000`.

## Usage

### Roles
- Lecturers: Submit claims, upload documents, and track statuses.
- Coordinators/Managers: Verify and approve/reject claims.
- HR Personnel: Generate reports and manage lecturer data.

### Navigation
- Home Page: Quick links to login and access role-specific views.
- Lecturer Dashboard: Create, submit, and track claims.
- Admin Dashboard: View pending claims, process approvals, and manage data.

## Development

### Version Control
This project follows best practices for version control:
- Frequent commits with descriptive messages.
- GitHub repository for collaboration and code versioning.

### Testing
Run unit tests to ensure reliability of critical functionalities:
```bash
dotnet test
```

## Future Enhancements
- Real-time notifications for claim status updates.
- Advanced reporting and analytics capabilities.
- Enhanced user experience through progressive web app (PWA) features.

## Contributors
- [Your Name]
- [Your Team Members]

## License
This project is licensed under the [MIT License](LICENSE).

### MIT License

```
MIT License

Copyright (c) 2024 CMCS

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

# Code Attribution:

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

