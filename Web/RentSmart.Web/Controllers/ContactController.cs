namespace RentSmart.Web.Controllers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RentSmart.Common;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Services.Messaging;
    using RentSmart.Web.ViewModels.Contact;

    using static RentSmart.Common.EntityValidationConstants;
    using static RentSmart.Common.GlobalConstants;
    using static RentSmart.Common.NotificationConstants;

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

            var html = new StringBuilder();
            html.AppendLine($"<h3>Thank you for contacting us!</h3>");
            html.AppendLine($"<p>Dear {contactForm.Name},</p>");
            html.AppendLine($"<p>Thanks for getting in touch! We’ve received your message and will get back to you as soon as we can — usually within 3 days.</p>");

            await this.emailSender.SendEmailAsync(
                 SystemEmailSender,
                 "RentSmart",
                 contactForm.Email,
                 "Thank you for contacting us!",
                 html.ToString());

            this.TempData[SuccessMessage] = "Thank you for contacting us! You can expect a response at the email you provided!";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
