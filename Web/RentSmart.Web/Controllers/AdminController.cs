﻿#pragma warning disable
namespace RentSmart.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data;
    using RentSmart.Data.Models;
    using System.Linq;

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;

        public AdminController(ApplicationDbContext db)
        {
            this.db = db;
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
    }
}
