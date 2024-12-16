namespace RentSmart.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;

    public class UserController : AdministrationController
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Manager> managerRepository;

        public UserController(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Manager> managerRepository)
        {
            this.userRepository = userRepository;
            this.managerRepository = managerRepository;
        }

        public async Task<IActionResult> Index()
        {
            return this.View(await this.userRepository.AllWithDeleted().OrderBy(x => x.IsDeleted)
                .ThenBy(x => x.UserName).Include(x => x.Manager)
                .Include(x => x.Owner).Include(x => x.Renter).ToListAsync());
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.userRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await this.userRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            user.FirstName = "...";
            user.LastName = "...";
            user.UserName = "...";
            user.Email = "...";
            this.userRepository.Delete(user);

            await this.userRepository.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> MakeManager(string? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.userRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }

        [HttpPost]
        [ActionName("MakeManager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeManagerConfirmed(string id)
        {
            var user = await this.userRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (user != null && user.Manager == null)
            {
                var manager = new Manager { User = user, UserId = user.Id };
                await this.managerRepository.AddAsync(manager);
                await this.managerRepository.SaveChangesAsync();
            }

            await this.userRepository.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
