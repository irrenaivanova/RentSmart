namespace RentSmart.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        [Authorize]
        protected string GetUserId()
        {
            return this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
