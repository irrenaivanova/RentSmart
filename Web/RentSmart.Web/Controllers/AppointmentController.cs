namespace RentSmart.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Services.Data;
    using RentSmart.Web.ViewModels.Appointments;

    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [Authorize]
        [HttpGet("GetAvailableHours")]
        public async Task<ActionResult<List<string>>> GetAvailableHours([FromQuery] string propertyId, [FromQuery] string dateChosen)
        {
            var hours = await this.appointmentService.GetAvailableHoursAsync(propertyId, dateChosen);
            return this.Ok(hours);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AppointmentData input)
        {
            string userId = this.GetUserId();
            try
            {
                await this.appointmentService.CreateAppointmentAsync(input.PropertyId, input.Date, input.Time, userId);
                return this.Ok();
            }
            catch (Exception)
            {
                return this.BadRequest("There was an error creating the appointment.");
            }
        }
    }
}
