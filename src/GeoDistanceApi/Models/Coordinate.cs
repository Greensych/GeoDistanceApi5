using System.ComponentModel.DataAnnotations;

namespace GeoDistanceApi.Models;

public class Coordinate
{
    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    public bool IsValid() => 
        Latitude >= -90 && Latitude <= 90 && 
        Longitude >= -180 && Longitude <= 180;
}
