using EndangeredNearYou.Domain.Interfaces;

namespace EndangeredNearYou.Infrastructure.Classes
{
    public class Location : ILocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}