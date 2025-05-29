using CarDealership.Data;
using CarDealership.Models;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetCars()
        {
            var carViewModel = new CarViewModel
            {
                Cars = await _context.Cars.ToListAsync() // Fetch data from DB
            };
            return Ok(carViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Route("api/addcar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCar(CarViewModel carvm, [FromForm] IFormFile file)
        {
            if (file == null || file.Length <= 0 || string.IsNullOrEmpty(file.FileName))
            {
                return Ok(carvm);
            }

            if (!ModelState.IsValid)
            {
                return Ok(carvm);
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
            return Ok(carvm);
        }


        public async Task<IActionResult> Details(string id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound(); // Handle case where car doesn't exist
            }
            return View(car); // Pass car to the view
        }

        [HttpGet]
        [Route("api/getcar")]
        public async Task<IActionResult> GetCar(string id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound(); // Handle case where car doesn't exist
            }
            return Ok(car); // Pass car to the view
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound(); // Handle case where car doesn't exist
            }
            return View(car); // Pass car to the view
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveEdit(Car car, [FromForm] IFormFile file)
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
            _context.Update(car);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = car.Id });
        }

        [HttpPost]
        [Route("api/editcar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveCarEdit(Car car, [FromForm] IFormFile file)
        {
            if (file == null || file.Length <= 0 || string.IsNullOrEmpty(file.FileName))
            {
                return Ok(car);
            }

            if (!ModelState.IsValid)
            {
                return Ok(car);
            }

            var path = Path.Combine("wwwroot", file.FileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }
            car.PictureUrl = path;
            _context.Update(car);
            await _context.SaveChangesAsync();

            return Ok("Car updated successfully");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                // Construct file path
                string imagePath = car.PictureUrl;

                // Check if the image file exists before attempting to delete
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath); // Delete the file
                }


                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index"); // Go back to car list after deleting
        }

        [HttpPost]
        [Route("api/deletecar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(string id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                // Construct file path
                string imagePath = car.PictureUrl;

                // Check if the image file exists before attempting to delete
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath); // Delete the file
                }

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
            return Ok("Car deleted successfully"); // Go back to car list after deleting
        }
    }
}
