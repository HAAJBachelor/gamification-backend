using gamification_backend.Models;

namespace gamification_backend.Utility;

public class TimerDepletedEventArgs : EventArgs
{
    public TimerDepletedEventArgs(SessionRecord record)
    {
        this.record = record;
    }

    public SessionRecord record { get; set; }
}