using CarDealership.Data;
using CarDealership.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers
{
    public class DealershipController : Controller
    {
        private readonly ILogger<DealershipController> _logger;
        private readonly CarsDbContext _context;


        public DealershipController(ILogger<DealershipController> logger, CarsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            if (cars.Count > 0)
            {
                return View(cars);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Route("api/getcars")]
        public async Task<IActionResult> GetCarsAPI()
        {
            var cars = await _context.Cars.ToListAsync();
            if (cars.Count > 0)
            {
                return Ok(cars);
            }
            else
            {
                return Ok("Dealership is empty");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(Car car, [FromForm] IFormFile file)
        {
            if (file == null || file.Length <= 0 || string.IsNullOrEmpty(file.FileName))
            {
                return View(car);
            }

            if (!ModelState.IsValid)
            {
                return View(car);
            }

            var path = Path.Combine("wwwroot", file.FileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }
            car.PictureUrl = path;
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("api/uploadpictures")]
        public async Task<IActionResult> UploadCar(Car car, [FromForm] IFormFile file)
        {
            if (file == null || file.Length <= 0 || string.IsNullOrEmpty(file.FileName))
            {
                return View(car);
            }

            if (!ModelState.IsValid)
            {
                return View(car);
            }

            var path = Path.Combine("wwwroot", file.FileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }
            car.PictureUrl = path;
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
