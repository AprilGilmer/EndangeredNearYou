using EndangeredNearYou.Infrastructure.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace EndangeredNearYou.Infrastructure.Services
{
    public class INaturalistApiClient
    {
        // Fields
        private readonly HttpClient _httpClient;
        private readonly string _key;

        // Properties
        private const bool captive = false;
        private const bool identified = true;
        private const bool photos = true;
        private const bool taxon_is_active = true;
        private const bool threatened = true;
        private const string cs = "GX,GH,G1,G2,G3,VU,EN,CR,EW,EX"; // Conservation Status Codes
        private const string radius = "50";
        private const string quality_grade = "research";
        private const bool include_ancestors = false;

        //CURL: curl -X GET --header 'Accept: application/json' 'https://api.inaturalist.org/v1/observations/species_counts?captive=false&identified=true&photos=true&threatened=true&hrank=genus&lrank=subspecies&lat=80.543643&lng=43.125445&radius=80&quality_grade=research&include_ancestors=false'

        // Constructor
        public INaturalistApiClient(HttpClient httpClient, AppSettings key)
        {
            _httpClient = httpClient;
            _key = key.ApiKey;
        }

        public async Task<List<Observations_SpeciesCounts>> GetObservations_SpeciesCountsAsync(double lat, double lng)
        {
            // Build the URL to send Http Requests to
            var url = $"https://api.inaturalist.org/v1/observations/species_counts?captive={captive}&identified={identified}&photos={photos}&taxon_is_active={taxon_is_active}&threatened={threatened}&cs={cs}&lat={lat}&lng={lng}&radius={radius}&quality_grade={quality_grade}&include_ancestors={include_ancestors}".ToLower();

            // Encode the URL due to question marks causing errors when making requests
            var encodedUrl = HttpUtility.UrlEncode(url);

            // Send request and recieve response from API
            var response = _httpClient.GetStringAsync(url).Result;

            // Deserialize json
            var results = JObject.Parse(response).GetValue("results")?.ToString();
            var model = JsonConvert.DeserializeObject<List<Observations_SpeciesCounts>>(results);

            if (model == null || model.Count() <= 0)
            {
                return model;
            }
            
            // NatureServe status names are only showing as codes, so we are populating the names manually based on the code
            foreach (var species in model)
            {
                if (species.taxon.conservation_Status != null && !string.IsNullOrEmpty(species.taxon.conservation_Status.authority)
                    && species.taxon.conservation_Status.authority.ToLower() != "iucn red list" && !string.IsNullOrEmpty(species.taxon.conservation_Status.status))
                {
                    switch (species.taxon.conservation_Status.status.ToUpper())
                    {
                        case "GX":
                            species.taxon.conservation_Status.status_name = "Presumed Extinct";
                            break;
                        case "GH":
                            species.taxon.conservation_Status.status_name = "Possibly Extinct";
                            break;
                        case "G1":
                            species.taxon.conservation_Status.status_name = "Critically Imperiled";
                            break;
                        case "G2":
                            species.taxon.conservation_Status.status_name = "Imperiled";
                            break;
                        case "G3":
                            species.taxon.conservation_Status.status_name = "Vulnerable";
                            break;
                        case "G4":
                            species.taxon.conservation_Status.status_name = "Apparently Secure";
                            break;
                        case "G5":
                            species.taxon.conservation_Status.status_name = "Secure";
                            break;
                        case "":
                            species.taxon.conservation_Status.status_name = "No Status Rank";
                            break;
                        default:
                            break;
                    }
                }
            }

            return model.OrderBy(x => x.taxon.preferred_common_name).ToList();
        }
    }
}