using System;

namespace TrackGenerator
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Press the Enter key to stop timer... ");

      var trackGenerator = new TrackGenerator();
      trackGenerator.BeginGenerator(10);

      Console.ReadLine();

      trackGenerator.EndGenerator();

      Console.WriteLine("Press the Enter key to exit... ");
      Console.ReadLine();
    }
  }
}
