using Dapper;
using EndangeredNearYou.Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace EndangeredNearYou.Domain.Repositories
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetAllLocations();
        Location GetLocationById(int id);
        void UpdateLocation(Location location);
        void InsertLocation(Location locationToInsert);
        IEnumerable<Category> GetCategories();
        Location AssignCategory();
        void DeleteLocation(Location location);
    }

    public class LocationRepository : ILocationRepository
    {
        private readonly IDbConnection _conn;
        public LocationRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return _conn.Query<Location>("Select * FROM world_cities");
        }

        public Location GetLocationById(int id)
        {
            return _conn.QuerySingle<Location>("SELECT * FROM world_cities WHERE cityId = @id", new { id });
        }

        public void UpdateLocation(Location location)
        {
            _conn.Execute("UPDATE world_cities SET Name = @name, Price = @price WHERE cityId = @id",
             new { 
                 name = location.Name, 
                 price = location.Price, 
                 id = location.cityId
             });
        }

        public void InsertLocation(Location locationToInsert)
        {
            _conn.Execute("INSERT INTO world_cities (NAME, PRICE, CATEGORYID) VALUES (@name, @price, @categoryID);",
                new { 
                    name = locationToInsert.Name, 
                    price = locationToInsert.Price, 
                    categoryID = locationToInsert.CategoryId 
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
            location.Categories = categoryList;

            return location;
        }

        public void DeleteLocation(Location location)
        {
            _conn.Execute("DELETE FROM REVIEWS WHERE cityId = @id;", 
                new {
                    id = location.cityId
                });
            _conn.Execute("DELETE FROM Sales WHERE cityId = @id;", 
                new {
                    id = location.cityId
                });
            _conn.Execute("DELETE FROM world_cities WHERE cityId = @id;", 
                new { 
                    id = location.cityId
                });
        }
    }
}