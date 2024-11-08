namespace RentSmart.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class PropertyController : BaseController
    {
        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }
    }
}
