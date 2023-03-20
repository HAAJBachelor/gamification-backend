namespace gamification_backend.Utility;

public class EventArgsFromTimer
{
    public EventArgsFromTimer(int elapsed)
    {
        Elapsed = elapsed;
    }

    public int Elapsed { get; set; }
}