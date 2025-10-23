using EndangeredNearYou.Domain.Interfaces;
using EndangeredNearYou.Infrastructure.Classes;
using EndangeredNearYou.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

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
            // When the user selects a city, the coordinates are used in the iNaturalist API call
            // Then they are redirected to the table of species displayed in the index of the species controller
            var location = _locationRepository.GetLocationById(id);
            SetHttpContextData(location);
            return RedirectToAction("Index", "Species");
        }

        private void SetHttpContextData(ILocation city)
        {
            HttpContext.Session.SetString("UserLocationName", city.Name);
            HttpContext.Session.SetString("UserLatitude", city.Latitude.ToString());
            HttpContext.Session.SetString("UserLongitude", city.Longitude.ToString());
        }
    }
}