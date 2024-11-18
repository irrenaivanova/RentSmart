namespace RentSmart.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static RentSmart.Common.GlobalConstants;

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
            return this.User.HasClaim(ManagerClaim, "true");
        }

        [Authorize]
        protected bool IsRenter()
        {
            return this.User.HasClaim(RenterClaim, "true");
        }

        [Authorize]
        protected bool IsOwner()
        {
            return this.User.HasClaim(OwnerClaim, "true");
        }
    }
}
