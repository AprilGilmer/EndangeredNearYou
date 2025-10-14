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
            var total = _conn.QuerySingle<int>($"SELECT COUNT(*) FROM world_cities");
            var random = new Random();
            int id = random.Next(1, total + 1);
            return GetLocationById(id);
        }

        public void UpdateLocation(Location location)
        {
            _conn.Execute("UPDATE world_cities SET Name = @name, Price = @price WHERE cityId = @id",
             new
             {
                 name = location.State,
                 price = location.County,
                 id = location.City_Id
             });
        }

        public void InsertLocation(Location locationToInsert)
        {
            _conn.Execute("INSERT INTO world_cities (NAME, PRICE, CATEGORYID) VALUES (@name, @price, @categoryID);",
                new
                {
                    name = locationToInsert.State,
                    price = locationToInsert.County,
                    categoryID = locationToInsert.Name
                });
        }

        public IEnumerable<Category> GetCategories()
        {
            return _conn.Query<Category>("SELECT * FROM categories;");
        }

        public Location AssignCategory()
        {
            var categoryList = GetCategories();
            var location = new Location();

            return location;
        }

        public void DeleteLocation(Location location)
        {
            _conn.Execute("DELETE FROM REVIEWS WHERE cityId = @id;",
                new
                {
                    id = location.City_Id
                });
            _conn.Execute("DELETE FROM Sales WHERE cityId = @id;",
                new
                {
                    id = location.City_Id
                });
            _conn.Execute("DELETE FROM world_cities WHERE cityId = @id;",
                new
                {
                    id = location.City_Id
                });
        }
    }
}