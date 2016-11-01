using UnityEngine;
using Zenject;

public class ScoreManager : ITickable
{
    public float InitialStartTime = 15f;
    private float _timeScore;
    private float _targetScore;
    private float _timeElapsed;
    private float _timeLimit;
    private float _timeElapsedForLevel;
    private float _expectedLevelTime;
    public float TimePointsLevelModifier = 0.2f;
    public float TimePointsPerSecond = 10.0f;
    public float TargetPointsLevelModifier = 0.2f;

    public int Score { get { return (int) (_timeScore + _targetScore); } }
    public int Level { get; private set; }

    public float TimeLeft
    {
        get { return _timeLimit - _timeElapsed; }
        set { _timeElapsed = TimeLeft - value; }
    }

    public bool GameOver { get { return TimeLeft > 0; }}

    public void AddTargetScore(float score)
    {
        _targetScore += score * TargetPointsLevelModifier;
    }

    public void NextLevel()
    {
        Level++;
        _timeScore += CalculateLevelTimeScore();
        _timeElapsedForLevel = 0;
        _expectedLevelTime = GetTimeForLevel();
        _timeLimit += GetTimeForLevel();
    }

    public bool IsTestLevel()
    {
        return false;
    }

    public void Tick()
    {
        _timeElapsed += Time.deltaTime;
        _timeElapsedForLevel += Time.deltaTime;
    }



    private float CalculateLevelTimeScore()
    {
        var spareTime = _timeElapsedForLevel - _expectedLevelTime;
        if (spareTime > 0)
        {
            return spareTime * TimePointsPerSecond * Level * TimePointsLevelModifier;
        }
        return 0f;
    }

    /// <summary>
    /// Calculate the time you gain for finishing the current level
    /// </summary>
    /// <returns>A float representing the time you gained in seconds</returns>
    private float GetTimeForLevel()
    {
        return 7.0f;
    }

    public void Reset()
    {
        _timeElapsed = 0;
        _timeElapsedForLevel = 0;
        _targetScore = 0;
        _timeScore = 0;
    }
}