using EndangeredNearYou.Domain.Entities;
using EndangeredNearYou.Infrastructure.Classes;
using System.Collections.Generic;

namespace EndangeredNearYou.Domain.Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<Continent> GetAllContinents();
        IEnumerable<Country> GetCountriesByContinent(string continent);
        IEnumerable<Location> GetLocationsByCountry(string country);
        Location GetLocationById(int id);
        Location GetRandomLocation();
    }
}
