namespace gamification_backend.DTO;

public class StateDTO
{
    public StateDTO(int points, int score, int time)
    {
        _points = points;
        _score = score;
        _time = time;
    }

    private int _points { get; set; }
    private int _score { get; set; }

    private int _time { get; }
}