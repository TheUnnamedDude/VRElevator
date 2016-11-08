using UnityEngine;
using Zenject;

public class GameController : ITickable
{
    [Inject]
    private ScoreManager _scoreManager;

    [Inject]
    private LevelGenerator _levelGenerator;

    private bool _running = true;
    private float _slowMotionTimeLeft;

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
        if (_slowMotionTimeLeft <= 0)
        {
            _slowMotionTimeLeft -= Time.deltaTime;
            Time.timeScale = 1f;
        }
    }

    public void SetSlowMotion(float time)
    {
        _slowMotionTimeLeft = time;
        Time.timeScale = 0.5f;
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
