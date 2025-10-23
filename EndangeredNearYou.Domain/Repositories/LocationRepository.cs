using Dapper;
using EndangeredNearYou.Domain.Entities;
using EndangeredNearYou.Domain.Interfaces;
using EndangeredNearYou.Infrastructure.Classes;
using System;
using System.Collections.Generic;
using System.Data;

namespace EndangeredNearYou.Domain.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IDbConnection _conn;
        public LocationRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<Continent> GetAllContinents()
        {
            return _conn.Query<Continent>(
                "SELECT DISTINCT Continent_Name, Continent_Code FROM country_continent_codes;"
            );
        }

        public IEnumerable<Country> GetCountriesByContinent(string continent)
        {
            return _conn.Query<Country>($"SELECT * FROM country_continent_codes WHERE Continent_Code = @continent ORDER BY Country_Name", new { continent });
        }

        public IEnumerable<Location> GetLocationsByCountry(string country)
        {
            return _conn.Query<Location>($"SELECT * FROM world_cities WHERE Country_Code = @country ORDER BY Name", new { country });
        }

        public Location GetLocationById(int id)
        {
            return _conn.QuerySingle<Location>($"SELECT * FROM world_cities WHERE City_Id = @id", new { id });
        }

        public Location GetRandomLocation()
        {
            // Query the total # of cities in the cities table
            var total = _conn.QuerySingle<int>($"SELECT COUNT(*) FROM world_cities");
            var random = new Random();
            // Pick a random number between 1 & the total # of cities in the table
            int id = random.Next(1, total + 1);
            // Call the get location by id method using the random # as the location id
            return GetLocationById(id);
        }
    }
}