using System.Linq;
using UnityEngine;
using Zenject;

public class GameController : IInitializable, ITickable
{
    [Inject]
    private ScoreManager _scoreManager;

    [Inject]
    private LevelGenerator _levelGenerator;

    public bool IsRunning
    {
        get;
        set;
    }

    public void Initialize ()
    {
        IsRunning = true;
        _levelGenerator.Reset();
    }

    public void Tick ()
    {
        if (_scoreManager.GameOver)
        {
              // TODO: Redo this
        }
    }

    public void OnTargetDestroy()
    {
        _scoreManager.AddTargetScore(10.0f); // TODO: Pass target type
        if (NumberOfTargetsAlive <= 0)
        {
            _levelGenerator.FinishLevel();
        }
    }

    private int NumberOfTargetsAlive
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("Target")
                .Select(target => target.GetComponent<TargetBehaviour>())
                .Count(targetScript => targetScript.Alive);
        }
    }
}
