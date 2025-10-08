using EndangeredNearYou.Domain.Interfaces;
using System.Collections.Generic;

namespace EndangeredNearYou.Domain.Entities
{
    public class Location : ILocation
    {
        public int City_Id { get; set; }
        public string Country_Code { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}