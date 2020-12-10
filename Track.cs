using System;
using System.Net.Mime;

namespace TrackGenerator
{
  public class Track
  {
    public string TrackName { get; }
    public Point CurrentPoint { get; set; }
    public Point EndingPoint { get; set; }
    public double Elevation { get; set; }
    
    public bool CompletedTrack { get; private set; }

    public Track(string trackName, double startingLatitude, double startingLongitude, double endingLatitude, double endingLongitude, double elevation)
    {
      this.CompletedTrack = false;
      this.TrackName = trackName;
      this.CurrentPoint = new Point(startingLatitude, startingLongitude);
      this.EndingPoint = new Point(endingLatitude, endingLongitude);
      this.Elevation = elevation;
    }

    public void OutputTrackInformation()
    {
      if (!this.CompletedTrack)
      {
        var wrappedLongitude = Geospatial.Wrap180(this.CurrentPoint.Longitude);
        var wrappedLatitude = Geospatial.Wrap90(this.CurrentPoint.Latitude);

        Console.WriteLine($"{wrappedLatitude},{wrappedLongitude},{this.TrackName},#FF0000");
      }
    }

    public void MoveBy(double distance)
    {
      if (Geospatial.DistanceBetweenPoints(this.CurrentPoint, this.EndingPoint) <= distance)
      {
        this.CompletedTrack = true;
        return;
      }

      var bearing = Geospatial.ResolveBearingBetweenTwoPoints(this.CurrentPoint, this.EndingPoint);
      this.CurrentPoint = Geospatial.AddMetersToPointGivenBearingAndDistance(this.CurrentPoint, bearing, distance);
    }
  }
}
