namespace gamification_backend.Utility;

public class EventArgsFromTimer
{
    public EventArgsFromTimer(int seconds, int startTime)
    {
        Seconds = seconds;
        StartTime = startTime;
    }

    public int Seconds { get; set; }
    public int StartTime { get; set; }
}