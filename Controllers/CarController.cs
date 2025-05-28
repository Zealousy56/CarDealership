using CarDealership.Data;
using CarDealership.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    public class CarController : Controller
    {
        private readonly Car _car;

        public CarController(Car car)
        {
            _car = car;
        }


        public IActionResult Index()
        {
            if (_car == null) { return View(); }
            else { return View(_car); }
        }
    }
}
