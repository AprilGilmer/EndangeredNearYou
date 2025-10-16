using EndangeredNearYou.Domain.Interfaces;
using EndangeredNearYou.Infrastructure.Classes;
using EndangeredNearYou.Web.Helpers;
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
            var continents = _locationRepository.GetAllContinents();
            return View(continents);
        }

        public IActionResult ShareLocation()
        {
            return View();
        }

        [HttpPost("Location/SaveLocation")]
        public IActionResult SaveLocation([FromBody] Location location)
        {
            location.Name = "your area";
            SetHttpContextData(location);

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

        public IActionResult Random()
        {
            var location = _locationRepository.GetRandomLocation();
            SetHttpContextData(location);
            return RedirectToAction("Index", "Species");
        }

        [HttpGet]
        [Route("Location/ViewCountries/{continentCode}")]
        public IActionResult ViewCountries(string continentCode)
        { 
            var countries = _locationRepository.GetCountriesByContinent(continentCode);
            return View(countries);
        }

        [HttpGet]
        [Route("Location/ViewCitiesByCountry/{countryCode}")]
        public IActionResult ViewCitiesByCountry(string countryCode)
        {
            var cities = _locationRepository.GetLocationsByCountry(countryCode);
            var model = ViewModelMapper.ToLocationViewModelList(cities);
            return View(model);
        }

        public IActionResult SelectLocation(int id)
        {
            var location = _locationRepository.GetLocationById(id);
            SetHttpContextData(location);
            return RedirectToAction("Index", "Species");
        }

        [HttpPut]
        public IActionResult UpdateLocation(int id)
        {
            var location = _locationRepository.GetLocationById(id);
            if (location == null)
            {
                return View("LocationNotFound");
            }
            return View(location);
        }

        private void SetHttpContextData(ILocation city)
        {
            HttpContext.Session.SetString("UserLocationName", city.Name);
            HttpContext.Session.SetString("UserLatitude", city.Latitude.ToString());
            HttpContext.Session.SetString("UserLongitude", city.Longitude.ToString());
        }
    }
}