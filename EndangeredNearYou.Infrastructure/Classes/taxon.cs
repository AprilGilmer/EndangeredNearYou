namespace EndangeredNearYou.Infrastructure.Classes
{
    public class taxon
    {
        public string name { get; set; }
        public bool extinct { get; set; }
        public default_photo default_photo { get; set; }
        public int observations_count { get; set; }
        public string preferred_common_name { get; set; }
        public conservation_status conservation_Status { get; set; }
    }
}
