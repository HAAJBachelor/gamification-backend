namespace gamification_backend.Models;

public class TaskRewards
{
    public TaskRewards(int lives, int time)
    {
        Lives = lives;
        Time = time;
    }

    public int Lives { get; set; }

    public int Time { get; set; }

    public int Points { get; set; }
}