using System.Threading.Tasks.Sources;

namespace GamificationBackend.Models;

public class State
{
    private int _points { get; set; }
    private int _score { get; set; }

    private Timer _timer;

    public State(int points, int score, Timer timer)
    {
        _points = points;
        _score = score;
        _timer = timer;
    }
}