using System.Linq;
using UnityEngine;
using Zenject;

public class GameController : ITickable
{
    [Inject]
    private ScoreManager _scoreManager;

    [Inject]
    private LevelGenerator _levelGenerator;

    private bool _running = true;

    public bool IsRunning
    {
        get { return _running; }
        set { _running = value;  }
    }

    public void Tick ()
    {
        if (_scoreManager.GameOver)
        {
              // TODO: Redo this
        }
    }

    public void OnTargetDestroy(float points)
    {
        _scoreManager.AddTargetScore(points); // TODO: Pass target type
        if (_levelGenerator.NumberOfTargetsAlive <= 0)
        {
            _levelGenerator.FinishLevel();
        }
    }
}
