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
                LocationId = location.cityId,
                Name = location.Name,
                Price = location.Price,
                CategoryId = location.CategoryId,
                OnSale = location.OnSale,
                StockLevel = location.StockLevel,
                Categories = location.Categories,
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
                cityId = model.LocationId,
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId,
                OnSale = model.OnSale,
                StockLevel = model.StockLevel,
                Categories = model.Categories,
            };

            return location;
        }
    }
}