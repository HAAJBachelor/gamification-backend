namespace gamification_backend.DTO;

public class StateDTO
{
    public StateDTO(int points, int lives, int time)
    {
        _points = points;
        _lives = lives;
        _time = time;
    }

    public int _points { get; set; }
    public int _lives { get; set; }

    public int _time { get; }
}