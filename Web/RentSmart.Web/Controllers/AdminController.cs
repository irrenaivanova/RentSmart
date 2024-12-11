#pragma warning disable
namespace RentSmart.Web.Controllers
{
    using Hangfire;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data;
    using RentSmart.Data.Models;
    using RentSmart.Services.Messaging;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IEmailSender sender;

        public AdminController(ApplicationDbContext db, IEmailSender sender)
        {
            this.db = db;
            this.sender = sender;
        }
        public IActionResult Index()
        {
            var user = db.Users
                .Include(u => u.Manager) 
                .FirstOrDefault(x => x.Id == "77a54852-30f7-490a-8a2d-2e7f27ca2a00");
           
            if (user != null && user.Manager == null)
            {
                var manager = new Manager { User = user, UserId = user.Id };
                db.Managers.Add(manager);
                db.SaveChanges();
                var managerAdd = db.Managers.FirstOrDefault(x => x.UserId == user.Id);
                user.Manager = managerAdd;
                db.SaveChanges();

            }
            return RedirectToAction("/");
        }

        [HttpGet]
        public async Task<IActionResult> SendToEmail()
        {
            var html = new StringBuilder();
            html.AppendLine($"<h1>Name</h1>");
            html.AppendLine($"<h3>something</h3>");
            await this.sender.SendEmailAsync("recepti@recepti.com", "MoiteRecepti", "irrenaivanova@gmail.com", "name", html.ToString());
            return RedirectToAction("Index","Home");
        }

        public IActionResult HangFire()
        {
            // Create a background job
            BackgroundJob.Enqueue(() => Console.WriteLine("This is a background job!"));

            // Create a recurring job
            RecurringJob.AddOrUpdate(() => Console.WriteLine("This job runs periodically."), Cron.Minutely);

            return View();
        }
    }
}
