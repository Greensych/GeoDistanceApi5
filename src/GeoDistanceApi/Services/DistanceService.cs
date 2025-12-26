using GeoDistanceApi.Models;

namespace GeoDistanceApi.Services;

public class DistanceService : IDistanceService
{
    private readonly ILogger<DistanceService> _logger;
    private const double EarthRadiusKm = 6371.0;

    public DistanceService(ILogger<DistanceService> logger)
    {
        _logger = logger;
    }

    public double CalculateDistance(Coordinate from, Coordinate to)
    {
        if (from == null || to == null)
            throw new ArgumentNullException();

        var lat1 = ToRadians(from.Latitude);
        var lat2 = ToRadians(to.Latitude);
        var dLat = ToRadians(to.Latitude - from.Latitude);
        var dLon = ToRadians(to.Longitude - from.Longitude);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}
