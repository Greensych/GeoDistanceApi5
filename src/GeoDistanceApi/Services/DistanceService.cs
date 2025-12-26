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
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (!from.IsValid() || !to.IsValid())
        {
            _logger.LogWarning("Invalid coordinates: From ({FromLat}, {FromLon}), To ({ToLat}, {ToLon})",
                from.Latitude, from.Longitude, to.Latitude, to.Longitude);
            throw new ArgumentException("Coordinates out of valid range");
        }

        var lat1 = ToRadians(from.Latitude);
        var lat2 = ToRadians(to.Latitude);
        var dLat = ToRadians(to.Latitude - from.Latitude);
        var dLon = ToRadians(to.Longitude - from.Longitude);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = EarthRadiusKm * c;

        _logger.LogDebug("Calculated distance: {Distance} km", distance);
        return distance;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}
