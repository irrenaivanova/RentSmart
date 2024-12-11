namespace RentSmart.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Common;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Messaging;
    using RentSmart.Web.ViewModels.Contact;

    using static RentSmart.Common.NotificationConstants;
    using static RentSmart.Common.GlobalConstants;
    using System;

    public class ContactController : BaseController
    {
        private readonly IRepository<ContactForm> contactsRepository;

        private readonly IEmailSender emailSender;

        public ContactController(IRepository<ContactForm> contactsRepository, IEmailSender emailSender)
        {
            this.contactsRepository = contactsRepository;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactFormViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var contactForm = new ContactForm
            {
                Name = model.Name,
                Email = model.Email,
                Title = model.Title,
                Content = $"{model.Email} ({model.Name}) send: Title:{model.Title} {Environment.NewLine} {model.Content}",
            };
            await this.contactsRepository.AddAsync(contactForm);
            await this.contactsRepository.SaveChangesAsync();

            await this.emailSender.SendEmailAsync(
                SystemEmailSender,
                "Contact Form RentSmart",
                SystemEmailReceiver,
                model.Title,
                contactForm.Content);

            this.TempData[SuccessMessage] = "Thank you for contacting us! You can expect a response at the email you provided!";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
