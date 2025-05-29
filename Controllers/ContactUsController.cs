using CarDealership.Models;
using CarDealership.Services;
using CarDealership.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly EmailService _emailService;
        private readonly CarsDbContext _context;

        public ContactUsController(CarsDbContext context, EmailService emailService)
        {
            _emailService = emailService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(new ContactUs());
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm(ContactUs contact)
        {
            await _emailService.SendEmailAsync(contact.Email, contact.Name, contact.Subject, contact.Message);
            _context.Inquiries.Add(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction("ThankYou"); // Redirect after sending
        }

        [HttpPost]
        [Route("api/contactus/submitform")]
        public async Task<IActionResult> SubmitFormAPI(ContactUs contact)
        {
            await _emailService.SendEmailAsync(contact.Email, contact.Name, contact.Subject, contact.Message);
            _context.Inquiries.Add(contact);
            await _context.SaveChangesAsync();
            return Ok("Email Sent");
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
