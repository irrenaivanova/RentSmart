# ASP.NET Core RentSmart
## 🏡 Project Introduction
**RentSmart** is ready-to-use ASP.NET Core 8 application and is the final project for my study in SoftUni

## 📝 Project Description


##  🛠️ Tehnology Stack:
**Backend:**
- ASP.NEt Core 8.0
- Entity Framework Core 8.0
- SQL Server
- HangFire
- SendGrid
- Rotativa

**Frontend:**
- Bootstrap
- JavaScript
- AJAX Real-Time Requests

**Libraries & APIs:**
- Google ReCAPTCHA
- Google for Developers
- Facebook for Developers
- Newtonsoft.Json
- HtmlSanitizer
- AngleSharp
- Font Awasome

**Testing:**
- xUnit
- Moq

## ⚙️ Background processes
**Hangfire** has one registered job
- Hangfire checks all contracts every day, and if a rental contract is set to expire within the next three days, the renter will receive a reminder email  

## 🔗 Link
[https://rentsmartweb20241216205226.azurewebsites.net/](https://rentsmartweb20241216205226.azurewebsites.net/)

**Note:** Unfortunately, by the time of submitting the project, the Facebook login button and Hangfire were not functioning properly. However, this issue does not occur when running the app locally. My first task for future development is to resolve this problem.

##  💾 Database Diagram


## 🤝 Credits
Using ASP.NET-MVC-Template originally developed by:
- [Nikolay Kostov](https://github.com/NikolayIT)
- [Vladislav Karamfilov](https://github.com/vladislav-karamfilov)


## 📸 Screenshots:
Home Page
<p align="center">
<img src="screenshots/homePage.png" alt="App Screenshot"  width="800">
</p>