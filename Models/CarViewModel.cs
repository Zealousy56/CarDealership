namespace CarDealership.Models
{
    public class CarViewModel
    {
        public List<Car> Cars { get; set; } = new List<Car>(); // Holds existing cars from DB
        public Car NewCar { get; set; } = new Car(); // Used for adding a new car
    }
}
