namespace RentSmart.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Services.Data;

    public class GetCountsComponent : ViewComponent
    {
        private readonly IGetCountsService getCountsService;

        public GetCountsComponent(IGetCountsService getCountsService)
        {
            this.getCountsService = getCountsService;
        }

        public IViewComponentResult Invoke()
        {
            var counts = this.getCountsService.GetCountsViewModel();
            return this.View(counts);
        }
    }
}
