using CarDealership.Models;
using CarDealership.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly EmailService _emailService;

        public ContactUsController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View(new ContactUs());
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm(ContactUs contact)
        {
            await _emailService.SendEmailAsync(contact.Email, contact.Name, contact.Subject, contact.Message);
            return RedirectToAction("ThankYou"); // Redirect after sending
        }

        [HttpPost]
        [Route("api/contactus/submitform")]
        public async Task<IActionResult> SubmitFormAPI(ContactUs contact)
        {
            await _emailService.SendEmailAsync(contact.Email, contact.Name, contact.Subject, contact.Message);
            return Ok("Email Sent");
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
