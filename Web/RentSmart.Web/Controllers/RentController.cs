namespace RentSmart.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Data.Models;

    public class RentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RentController(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        // When making arental - first chech if the user has bought package and then to make it not active as writing a property in it
        public IActionResult Index()
        {
            return this.View();
        }

        private async Task UpgradeToOwner(string userId)
        {
            // I do not handle exceptions
            var user = await this.userManager.FindByIdAsync(userId);
            var claims = await this.userManager.GetClaimsAsync(user);
            if (claims.Any(x => x.Type == "IsRenter" && x.Value == "true"))
            {
                await this.userManager.AddClaimAsync(user, new Claim("IsRenter", "true"));
            }
        }
    }
}
