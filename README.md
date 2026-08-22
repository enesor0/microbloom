# 🌱 microbloom

**microbloom** is a modern career platform built with **ASP.NET Core 8** and **Blazor Server**, designed to bring job seekers, employers, mentors, universities, and career development resources together in a single ecosystem.

The platform provides tools for discovering job opportunities, managing applications, accessing career resources, communicating with other users, exploring universities, and receiving mentorship support.

---

## ✨ Features

### 👤 For Job Seekers

* Browse and search job opportunities
* View detailed job postings
* Apply to job listings
* Track submitted applications
* Create and manage a personal profile
* Explore companies
* Access CV examples
* Prepare for interviews
* Explore salary information
* Use career guidance resources
* Take career-oriented tests
* Access industry analyses
* Follow career-related events and planning tools

### 🏢 For Employers

* Dedicated company dashboard
* Company profile management
* Create and manage job postings
* View applications
* Manage candidates
* Interact with job seekers

### 🎓 Career & Education

* Career guides and articles
* University exploration
* Department information
* CV samples
* Salary guide
* Industry analysis
* Interview preparation
* Career calendar
* Career test

### 🤝 Mentorship & Communication

* Browse mentors
* Apply for mentorship
* Mentor profiles
* Mentor dashboard
* Manage mentorship applications
* Built-in messaging system

---

## 🛠️ Tech Stack

| Technology                  | Purpose                          |
| --------------------------- | -------------------------------- |
| **C#**                      | Main programming language        |
| **.NET 8**                  | Application framework            |
| **ASP.NET Core**            | Backend and web infrastructure   |
| **Blazor Server**           | Interactive server-side UI       |
| **Entity Framework Core 8** | ORM and database access          |
| **SQL Server**              | Relational database              |
| **ASP.NET Core Identity**   | Authentication and authorization |
| **Razor Components**        | UI development                   |
| **Swagger / OpenAPI**       | API documentation                |
| **Bootstrap / CSS**         | Responsive UI styling            |

---

## 🏗️ Architecture

microbloom follows a layered structure that separates presentation, business logic, data access, and domain models.

```text
microbloom/
│
├── Controllers/
│   ├── AccountController.cs
│   ├── CompanyController.cs
│   ├── ContentController.cs
│   ├── CvSampleController.cs
│   ├── DepartmentController.cs
│   ├── JobPostingController.cs
│   ├── ProfileController.cs
│   └── UniversityController.cs
│
├── DTOs/
│   └── Data Transfer Objects
│
├── Data/
│   ├── Database Context
│   └── Database Seeders
│
├── Entities/
│   ├── AppUser.cs
│   ├── ContentArticle.cs
│   ├── ContentCategory.cs
│   ├── CvSample.cs
│   ├── Department.cs
│   ├── MentorshipApplication.cs
│   ├── Message.cs
│   └── University.cs
│
├── Migrations/
│   └── Entity Framework migrations
│
├── Pages/
│   ├── Account/
│   ├── Company/
│   ├── Profile/
│   ├── Jobs.razor
│   ├── JobDetail.razor
│   ├── CompanyDashboard.razor
│   ├── CareerGuide.razor
│   ├── CareerTest.razor
│   ├── CareerCalendar.razor
│   ├── CvSamples.razor
│   ├── IndustryAnalysis.razor
│   ├── InterviewPrep.razor
│   ├── Mentorship.razor
│   ├── Messages.razor
│   ├── SalaryGuide.razor
│   ├── Universities.razor
│   └── ...
│
├── Services/
│   ├── Factory/
│   ├── Implementations/
│   └── Interfaces/
│
├── Shared/
│   └── Shared Blazor components
│
├── wwwroot/
│   ├── css/
│   ├── images/
│   └── static assets
│
├── Program.cs
├── appsettings.json
└── microbloom.csproj
```

---

## 🔐 Authentication & Authorization

The application uses **ASP.NET Core Identity** for authentication and role-based authorization.

Three main roles are available:

```text
JobSeeker
Employer
Admin
```

Identity is integrated with Entity Framework Core and SQL Server.

Authentication sessions use secure HTTP cookies with configurable expiration and authorization policies.

---

## 💾 Database

microbloom uses **SQL Server** together with **Entity Framework Core**.

The default development configuration uses SQL Server LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MicrobloomDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

You can replace this connection string with your own SQL Server instance.

---

## 🚀 Getting Started

### Prerequisites

Make sure the following tools are installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/)
* SQL Server or SQL Server LocalDB
* Git
* Visual Studio 2022, Rider, or VS Code

---

### 1. Clone the Repository

```bash
git clone https://github.com/enesor0/microbloom.git
```

Navigate to the project directory:

```bash
cd microbloom/microbloom
```

---

### 2. Restore Dependencies

```bash
dotnet restore
```

---

### 3. Configure the Database

Open:

```text
appsettings.json
```

Update `DefaultConnection` if you are not using SQL Server LocalDB.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_SQL_SERVER_CONNECTION_STRING"
  }
}
```

---

### 4. Apply Database Migrations

If Entity Framework CLI tools are not installed:

```bash
dotnet tool install --global dotnet-ef
```

Then apply the existing migrations:

```bash
dotnet ef database update
```

---

### 5. Run the Application

```bash
dotnet run
```

After the application starts, open the HTTPS address displayed in the terminal.

The default development configuration uses:

```text
https://localhost:7223
```

---

## 📚 API Documentation

Swagger is enabled while the application is running in the **Development** environment.

Navigate to:

```text
/swagger
```

to explore available API endpoints.

---

## 🌱 Database Seeding

During application startup, microbloom initializes the required application roles and executes its database seeding logic.

The following roles are automatically prepared:

```text
JobSeeker
Employer
Admin
```

This makes it easier to start the application with development data and test different user flows.

---

## 🧩 Service Layer

The application uses dependency injection and service abstractions to separate business logic from UI components and controllers.

Some of the core services include:

```text
IAuthService
IUserService
IUniversityService
IDepartmentService
ICvSampleService
IContentService
IJobService
ICompanyService
IMessageService
IMentorshipService
```

This structure keeps the project modular and makes individual features easier to maintain and extend.

---

## 🔄 Application Flow

```text
Blazor Pages / Controllers
          │
          ▼
     Service Layer
          │
          ▼
 Entity Framework Core
          │
          ▼
      SQL Server
```

ASP.NET Core Identity operates alongside this flow to manage authentication, authorization, users, and roles.

---

## 🎯 Project Goals

microbloom aims to create a centralized career ecosystem where users can:

* Discover career opportunities
* Connect with employers
* Track job applications
* Improve their professional skills
* Access career and education resources
* Connect with mentors
* Explore companies and universities

The project also demonstrates the implementation of a full-stack .NET application using modern backend architecture, authentication, relational data management, APIs, and interactive server-side UI components.

---

## 🤝 Contributing

Contributions, suggestions, and improvements are welcome.

To contribute:

1. Fork the repository
2. Create a new branch

```bash
git checkout -b feature/new-feature
```

3. Commit your changes

```bash
git commit -m "Add new feature"
```

4. Push the branch

```bash
git push origin feature/new-feature
```

5. Open a Pull Request

---

## 👨‍💻 Author

**Enes Or**

GitHub: [@enesor0](https://github.com/enesor0)

---

<p align="center">
  Built with ASP.NET Core, Blazor and ☕
</p>
