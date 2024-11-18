namespace RentSmart.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Data.Models;

    using static RentSmart.Common.GlobalConstants;

    [Authorize]
    public class RentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RentController(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        private async Task UpgradeToRenter(string userId)
        {
            // I do not handle exceptions
            var user = await this.userManager.FindByIdAsync(userId);
            var claims = await this.userManager.GetClaimsAsync(user);
            if (claims.Any(x => x.Type == RenterClaim && x.Value == "true"))
            {
                await this.userManager.AddClaimAsync(user, new Claim(RenterClaim, "true"));
            }
        }
    }
}
