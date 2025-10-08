using EndangeredNearYou.Domain.Entities;
using EndangeredNearYou.Web.Models;

namespace EndangeredNearYou.Web.Helpers
{
    public static class ViewModelMapper
    {
        public static LocationViewModel ToLocationViewModel(Location location)
        {
            var model = new LocationViewModel()
            {
                City_Id = location.City_Id,
                Name = location.Name,
                Country_Code = location.Country_Code,
                State = location.State,
                County = location.County,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };

            return model;
        }

        public static List<LocationViewModel> ToLocationViewModelList(IEnumerable<Location> locations)
        {
            List<LocationViewModel> listOfModels = new List<LocationViewModel>();

            foreach (var location in locations)
            {
                var model = ToLocationViewModel(location);

                listOfModels.Add(model);
            }

            return listOfModels;
        }

        public static Location ToLocationEntity(LocationViewModel model)
        {
            var location = new Location()
            {
                City_Id = model.City_Id,
                State = model.Country_Code,
                County = model.State,
                Name = model.County,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };

            return location;
        }
    }
}