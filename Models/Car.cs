﻿namespace CarDealership.Models
{
    public class Car
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Make { get; set; }
        public string Model {  get; set; }
        public string Description { get; set; }
        public int ReleaseYear {  get; set; }
        public int Price { get; set; }
        public string? PictureUrl { get; set; }
    }
}
