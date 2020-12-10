using System;
using System.Collections.Generic;
using System.Timers;

namespace TrackGenerator
{
  public class TrackGenerator
  {
    private Timer timer;
    private readonly List<Track> tracks;
    private readonly List<Track> completedTracks;

    public TrackGenerator()
    {
      this.tracks = new List<Track>();
      this.completedTracks = new List<Track>();
    }

    public void BeginGenerator(int timerTime)
    {
      // Init the first track, 
      tracks.Add(new Track("Track One", 50.857113542453654, -1.096509841674581, 50.80436215080627, -1.1112835346570147, 1000));

      // Init timer

      this.timer = new Timer(timerTime);
      this.timer.Elapsed += TrackGeneratorTimerElapsedEvent;
      this.timer.Enabled = true;
      this.timer.AutoReset = true;

      foreach (var track in tracks)
      {
        track.OutputTrackInformation();
      }
    }

    public void EndGenerator()
    {
      this.timer.Enabled = false;
      this.timer.Elapsed -= TrackGeneratorTimerElapsedEvent;
    }

    private void TrackGeneratorTimerElapsedEvent(object sender, ElapsedEventArgs e)
    {
      foreach (var track in tracks)
      {
        if (!track.CompletedTrack)
        {
          track.MoveBy(100);
          track.OutputTrackInformation();
        }
        else
        {
          if (this.completedTracks.Contains(track))
          {
            continue;
          }
          else
          {
            this.completedTracks.Add(track);
            Console.WriteLine($"Completed : {track.TrackName}");
          }
        }
      }
    }
  }
}
