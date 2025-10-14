namespace EndangeredNearYou.Domain.Interfaces
{
    public interface ILocation
    {
        string Name { get; }
        double Latitude { get; }
        double Longitude { get; }
    }
}
