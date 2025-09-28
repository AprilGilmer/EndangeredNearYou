using EndangeredNearYou.Infrastructure.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EndangeredNearYou.Infrastructure.Services
{
    public class INaturalistApiClient
    {
        // Fields
        private readonly HttpClient _httpClient;
        private readonly string _key;

        // Properties
        private bool captive = false;
        private bool identified = true;
        private bool photos = true;
        private bool taxon_is_active = true;
        private bool threatened = true;
        private string cs = "GX,GH,G1,G2,G3,VU,EN,CR,EW,EX"; // Conservation Status Codes
        private string radius = "50";
        private string quality_grade = "research";
        private bool include_ancestors = false;

        // lat: 30.29123
        // lng: -87.586645
        //CURL: curl -X GET --header 'Accept: application/json' 'https://api.inaturalist.org/v1/observations/species_counts?captive=false&identified=true&photos=true&threatened=true&hrank=genus&lrank=subspecies&lat=80.543643&lng=43.125445&radius=80&quality_grade=research&include_ancestors=false'

        // Constructor
        public INaturalistApiClient(HttpClient httpClient, AppSettings key)
        {
            _httpClient = httpClient;
            _key = key.ApiKey;
        }

        public async Task<List<Observations_SpeciesCounts>> GetObservations_SpeciesCountsAsync(double lat, double lng)
        {
            var url = $"https://api.inaturalist.org/v1/observations/species_counts?captive={captive}&identified={identified}&photos={photos}&taxon_is_active={taxon_is_active}&threatened={threatened}&cs={cs}&lat={lat}&lng={lng}&radius={radius}&quality_grade={quality_grade}&include_ancestors={include_ancestors}".ToLower();
            var encodedUrl = HttpUtility.UrlEncode(url);
            var response = _httpClient.GetStringAsync(url).Result;

            // Deserialize json
            var results = JObject.Parse(response).GetValue("results")?.ToString();
            var model = JsonConvert.DeserializeObject<List<Observations_SpeciesCounts>>(results);

            return model.OrderBy(x => x.taxon.preferred_common_name).ToList();
        }
    }
}