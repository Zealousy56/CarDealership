using CarDealership.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers
{
    public class DashboardController : Controller
    {
        private readonly CarsDbContext _context;
        public DashboardController(CarsDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var inqs = await _context.Inquiries.ToListAsync();
            return View(inqs);
        }

        [Route("api/getinquiries")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInqs()
        {
            var inqs = await _context.Inquiries.ToListAsync();
            return Ok(inqs);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            var inq = await _context.Inquiries.FindAsync(id);
            if (inq == null)
            {
                return NotFound(); 
            }
            return View(inq); 
        }

        [Route("api/getinquiry")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInq(string id)
        {
            var inq = await _context.Inquiries.FindAsync(id);
            if (inq == null)
            {
                return NotFound();
            }
            return Ok(inq);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var inq = await _context.Inquiries.FindAsync(id);
            if (inq != null)
            {
                _context.Inquiries.Remove(inq);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index"); 
        }

        [HttpPost]
        [Route("api/deleteinquiry")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInq(string id)
        {
            var inq = await _context.Inquiries.FindAsync(id);
            if (inq != null)
            {
                _context.Inquiries.Remove(inq);
                await _context.SaveChangesAsync();
            }
            return Ok("Inquiry Deleted");
        }
    }
}
