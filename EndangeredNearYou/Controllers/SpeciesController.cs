using EndangeredNearYou.Infrastructure.Classes;
using EndangeredNearYou.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EndangeredNearYou.Web.Controllers
{
    public class SpeciesController : Controller
    {
        private readonly AppSettings _appSettings;
        private INaturalistApiClient _iNaturalistApiClient;

        public SpeciesController(IOptions<AppSettings> key)
        {
            _appSettings = key.Value;
            var client = new HttpClient();
            _iNaturalistApiClient = new INaturalistApiClient(client, _appSettings);
        }

        [Route("Species/Index")]
        public async Task<IActionResult> Index()
        {
            var latString = HttpContext.Session.GetString("UserLatitude");
            var lngString = HttpContext.Session.GetString("UserLongitude");

            var isLatParsed = double.TryParse(latString, out double latDouble);
            var isLngParsed = double.TryParse(lngString, out double lngDouble);

            if (!isLatParsed || !isLngParsed)
            {
                throw new Exception();
            }
            
            var model = await _iNaturalistApiClient.GetObservations_SpeciesCountsAsync(latDouble, lngDouble);
            return View(model);
        }

        [HttpPost]
        public IActionResult Details(string speciesJson)
        {
            var species = System.Text.Json.JsonSerializer.Deserialize<Observations_SpeciesCounts>(speciesJson);
            return View(species);
        }
    }
}
