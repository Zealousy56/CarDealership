using CarDealership.Data;
using CarDealership.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Controllers
{
    public class DealershipController : Controller
    {
        private readonly CarsDbContext _context;


        public DealershipController(CarsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var carViewModel = new CarViewModel
            {
                Cars = await _context.Cars.ToListAsync() // Fetch data from DB
            };
            return View(carViewModel);
        }

        [HttpGet]
        [Route("api/getcars")]
        public async Task<IActionResult> GetCarsAPI()
        {
            var carViewModel = new CarViewModel
            {
                Cars = await _context.Cars.ToListAsync() // Fetch data from DB
            };
            return Ok(carViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CarViewModel carvm, [FromForm] IFormFile file)
        {
            if (file == null || file.Length <= 0 || string.IsNullOrEmpty(file.FileName))
            {
                return View(carvm);
            }

            if (!ModelState.IsValid)
            {
                return View(carvm);
            }

            var path = Path.Combine("wwwroot", file.FileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }
            carvm.NewCar.PictureUrl = path;
            _context.Cars.Add(carvm.NewCar);
            await _context.SaveChangesAsync();

            carvm.Cars = await _context.Cars.ToListAsync();
            return RedirectToAction("Index", carvm);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("api/uploadpictures")]
        public async Task<IActionResult> AddCar(Car car, [FromForm] IFormFile file)
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
