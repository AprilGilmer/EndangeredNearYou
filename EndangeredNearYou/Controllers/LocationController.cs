using EndangeredNearYou.Domain.Repositories;
using EndangeredNearYou.Infrastructure.Classes;
using EndangeredNearYou.Web.Helpers;
using EndangeredNearYou.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EndangeredNearYou.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public IActionResult Index()
        {
            var locations = _locationRepository.GetAllLocations();
            var models = ViewModelMapper.ToLocationViewModelList(locations);
            return View(models);
        }

        [HttpPost("Location/SaveLocation")]
        public IActionResult SaveLocation([FromBody] Location location)
        {
            HttpContext.Session.SetString("UserLatitude", location.Latitude.ToString());
            HttpContext.Session.SetString("UserLongitude", location.Longitude.ToString());

            return Ok(new { message = "Location saved in session!" });
        }

        [HttpGet]
        public IActionResult ShowLocation()
        {
            var latitude = HttpContext.Session.GetString("UserLatitude");
            var longitude = HttpContext.Session.GetString("UserLongitude");

            var json = HttpContext.Session.GetString("UserLocation");
            if (json == null)
            {
                return Content("No location stored for this session.");
            }

            var location = JsonSerializer.Deserialize<Location>(json);
            return Content($"Your stored location: Lat={location.Latitude}, Lon={location.Longitude}");
        }

        public IActionResult Random(int id)
        {
            var location = _locationRepository.GetLocationById(id);
            var model = ViewModelMapper.ToLocationViewModel(location);
            //return RedirectToAction("Index");
            return View(location);
        }

        public IActionResult ViewLocation(int id)
        {
            var location = _locationRepository.GetLocationById(id);
            var model = ViewModelMapper.ToLocationViewModel(location);
            return View(location);
        }

        public IActionResult UpdateLocation(int id)
        {
            var location = _locationRepository.GetLocationById(id);
            if (location == null)
            {
                return View("LocationNotFound");
            }
            return View(location);
        }

        public IActionResult UpdateLocationToDatabase(LocationViewModel model)
        {
            var location = _locationRepository.GetLocationById(model.LocationId);
            _locationRepository.UpdateLocation(location);

            model = ViewModelMapper.ToLocationViewModel(location);
            return RedirectToAction("ViewLocation", new { id = model.LocationId });
        }

        public IActionResult InsertLocation()
        {
            var location = _locationRepository.AssignCategory();
            return View(location);
        }

        public IActionResult InsertLocationToDatabase(LocationViewModel model)
        {
            var location = ViewModelMapper.ToLocationEntity(model);
            _locationRepository.InsertLocation(location);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteLocation(LocationViewModel model)
        {
            var location = ViewModelMapper.ToLocationEntity(model);
            _locationRepository.DeleteLocation(location);
            return RedirectToAction("Index");
        }
    }
}