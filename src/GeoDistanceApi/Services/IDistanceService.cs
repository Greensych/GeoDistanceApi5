using GeoDistanceApi.Models;

namespace GeoDistanceApi.Services;

public interface IDistanceService
{
    double CalculateDistance(Coordinate from, Coordinate to);
}
