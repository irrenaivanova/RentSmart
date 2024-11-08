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

        [Authorize]
        protected bool IsManager()
        {
            return this.User.HasClaim("IsManager", "true");
        }

        [Authorize]
        protected bool IsRenter()
        {
            return this.User.HasClaim("IsRenter", "true");
        }

        [Authorize]
        protected bool IsOwner()
        {
            return this.User.HasClaim("IsOwner", "true");
        }
    }
}
