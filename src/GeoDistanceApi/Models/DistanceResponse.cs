namespace GeoDistanceApi.Models;

public class DistanceResponse
{
    public double DistanceKm { get; set; }
    public Coordinate From { get; set; } = null!;
    public Coordinate To { get; set; } = null!;
}
