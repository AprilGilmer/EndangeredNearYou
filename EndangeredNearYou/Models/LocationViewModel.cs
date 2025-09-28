using EndangeredNearYou.Domain.Entities;

namespace EndangeredNearYou.Web.Models
{
    public class LocationViewModel
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int OnSale { get; set; }
        public int StockLevel { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}