using System;

namespace TrackGenerator
{
  public struct Point
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public double LatitudeRads
    {
      get
      {
        return Latitude * Math.PI / 180;
      }
    }

    public double LongitudeRads
    {
      get
      {
        return Longitude * Math.PI / 180;
      }
    }

    public Point(double latitude, double longitude)
    {
      Latitude = latitude;
      Longitude = longitude;
    }
  }
}
