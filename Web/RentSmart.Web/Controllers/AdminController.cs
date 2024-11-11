#pragma warning disable
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
                .FirstOrDefault(x => x.Id == "e322294b-0c38-4d37-a550-f35e33f475a3");
           
            if (user != null && user.Manager == null)
            {
                var manager = new Manager { User = user, UserId = user.Id };
                db.Managers.Add(manager);
                db.SaveChanges();
                var managerAdd = db.Managers.FirstOrDefault(x => x.UserId == user.Id);
                user.Manager = managerAdd;
                user.ManagerId = managerAdd.Id;
                db.SaveChanges();

            }
            return RedirectToAction("/");
        }
    }
}
