using System;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;

namespace TrackGenerator
{

  public static class Geospatial
  {

    private const double EARTH_RADIUS = 6371e3;

    public static double ResolveBearingBetweenTwoPoints(Point startPoint, Point endPoint)
    {

      var deltaLongitude = endPoint.LongitudeRads - startPoint.LongitudeRads;

      var y = Math.Sin(deltaLongitude) * Math.Cos(endPoint.LatitudeRads);
      var x = Math.Cos(startPoint.LatitudeRads) * Math.Sin(endPoint.LatitudeRads) - 
        Math.Sin(startPoint.LatitudeRads) * Math.Cos(endPoint.LatitudeRads) * Math.Cos(deltaLongitude);

      var theta = Math.Atan2(y, x);

      return (theta * 180 / Math.PI + 360) % 360;
    }

    public static Point AddMetersToPointGivenBearingAndDistance(Point currentPoint, double bearing, double distance)
    {
      var angularDistance = distance / EARTH_RADIUS;
      var bearingInRads = bearing * Math.PI / 180;

      var latRads = Math.Asin(Math.Sin(currentPoint.LatitudeRads) * Math.Cos(angularDistance) 
        + Math.Cos(currentPoint.LatitudeRads) * Math.Sin(angularDistance) * Math.Cos(bearingInRads));

      var longRads = currentPoint.LongitudeRads + Math.Atan2(Math.Sin(bearingInRads) * Math.Sin(angularDistance) * Math.Cos(currentPoint.LatitudeRads),
        Math.Cos(angularDistance) - Math.Sin(currentPoint.LatitudeRads) * Math.Sin(latRads));

      var latDegs = latRads * 180 / Math.PI;
      var longDegs = longRads * 180 / Math.PI;
      
      return new Point(latDegs, longDegs);
    }

    public static double DistanceBetweenPoints(Point currentPoint, Point endingPoint)
    {
      var deltaLatitude = endingPoint.LatitudeRads - currentPoint.LatitudeRads;
      var deltaLongitude = endingPoint.LongitudeRads - currentPoint.LongitudeRads;

      var a = Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) 
        + Math.Cos(currentPoint.LatitudeRads) * Math.Cos(endingPoint.LatitudeRads) 
        * Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2);

      var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

      return EARTH_RADIUS * c;
    }

    /// <summary>
    /// Ripped right out of movable-types.co.uk
    /// </summary>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static double Wrap180(double degrees)
    {
      if (-180 < degrees && degrees <= 180)
      {
        return degrees; // avoid rounding due to arithmetic ops if within range
      }

      return (degrees + 540) % 360 - 180; // sawtooth wave p:180, a:±180
    }

    /// <summary>
    /// Ripped right out of movable-types.co.uk
    /// </summary>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static double Wrap90(double degrees)
    {
      if (-90 <= degrees && degrees <= 90)
      {
        return degrees; // avoid rounding due to arithmetic ops if within range
      }

      return Math.Abs((degrees % 360 + 270) % 360 - 180) - 90; // triangle wave p:360 a:±90 TODO: fix e.g. -315°
    }
  }
}
