namespace EndangeredNearYou.Web.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int OnSale { get; set; }
        public int StockLevel { get; set; }
    }
}