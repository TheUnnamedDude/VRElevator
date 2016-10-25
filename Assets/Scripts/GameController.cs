using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour
{
    private readonly int START_LEVEL = 1;

    public Transform Scene;
    public Transform Elevator;
    public GameObject TargetGameObject;
    public ScoreDispalyController ScoreDisplayController;
    public float BaseTimePerLevel;
    public double MinDistance;
    public double MaxDistance;
    public float TimePointsLevelModifier = 0.2f;
    public float TimePointsPerSecond = 10.0f;
    public float TargetPointsLevelModifier = 0.2f;

    private System.Random _rng;
    private int _seed;

    private int _level = 1;
    private float _timeElapsed;
    private float _timeLimit;
    private float _timeScore;
    private float _targetScore;
    
    private float _timeElapsedForLevel = 0f;
    private float _expectedLevelTime;

    private readonly int MIN_FOV = 90;
    private readonly int MAX_FOV = 360;

    public int Seed
    {
        get
        {
            if (_seed == 0)
            {
                _seed = new System.Random().Next();
            }
            return _seed;
        }
        set { _seed = value; }
    }

    public bool IsRunning
    {
        get;
        set;
    }

    public int Score
    {
        get { return (int) (_timeScore + _targetScore); }
    }

    public float TimeLeft {
        get { return _timeLimit - _timeElapsed; }
    }

    public int Level {
        get { return _level; }
    }

    public Transform[] SpawnPoints;

    // Use this for initialization
    void Start ()
    {
        IsRunning = true;
        InitializeRound();
    }

    // Update is called once per frame
    void Update ()
    {
        //
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed > _timeLimit)
        {
            // TODO: Game over, handle this
        }
        _timeElapsedForLevel += Time.deltaTime;
    }

    public void InitializeRound()
    {
        _rng = new System.Random(Seed);
        FinishLevel();
    }

    public float CalculateLevelTimeScore()
    {
        var spareTime = _timeElapsedForLevel - _expectedLevelTime;
        if (spareTime > 0)
        {
            return (spareTime * TimePointsPerSecond) * (((float) _level) * TimePointsLevelModifier);
        } else {
            return 0f;
        }
    }

    public float CalculateScoreForTarget() { // TODO: Pass the target type as a parameter and add score based on it
        return 10.0f * (((float)_level) * TargetPointsLevelModifier);
    }

    public void OnTargetDestroy()
    {
        _targetScore += CalculateScoreForTarget();
        Debug.Log(GetNumberOfTargetsAlive());

        if (GetNumberOfTargetsAlive() <= 0)
        {
            FinishLevel();
        }
    }

    private int GetNumberOfTargetsAlive()
    {
        int targetsAlive = 0;
        foreach (var target in GameObject.FindGameObjectsWithTag("Target")) {
            TargetScript targetScript = target.GetComponent<TargetScript>();
            if (targetScript.Alive)
            {
                targetsAlive++;
            }
        }
        return targetsAlive;
    }

    public void FinishLevel()
    {
        if (_level > START_LEVEL)
        {
            _timeScore = CalculateLevelTimeScore();
            _timeElapsedForLevel = 0;
        }
        // Make sure we don't have any game objects left
        foreach (var gObj in GameObject.FindGameObjectsWithTag("Target"))
        {
            Destroy(gObj);
        }

        //Debug.Log("Starting spawn");
        List<ElevatorDirection> availableDirections = Enum.GetValues(typeof(ElevatorDirection))
            .Cast<ElevatorDirection>()
            .ToList();
        var directions = new ElevatorDirection[GetElevatorSidesForLevel()];
		Debug.Log("Starting level " + _level);
        for (int i = 0; i < directions.Length; i++)
        {
            var directionIndex = _rng.Next(availableDirections.Count);
			Debug.Log("Test " + directionIndex);
            directions[i] = availableDirections[directionIndex];
            availableDirections.RemoveAt(directionIndex);
            Debug.Log("Spawning targets at " + directions[i]);
        }

        _expectedLevelTime = GetTimeForLevel();
        _timeLimit += _expectedLevelTime;

        float floorY = GameObject.FindGameObjectWithTag("Floor").transform.position.y;
        int numberOfSpawns = GetTargetSpawnsForLevel();;
        for (int i = 0; i < numberOfSpawns; i++)
        {
            bool spawnFound = false;
            for (int j = 0; j < 3 && !spawnFound; j++)
            {
                // TODO: Floor position
                var direction = GetRandomDirection(directions);
                var rand = GetRandomPosition(direction);
                Vector3 spawnPosition = Elevator.position + rand;
                spawnPosition.y = floorY;
                if (IsValidSpawn(spawnPosition, TargetGameObject.GetComponentInChildren<Collider>().bounds.extents))
                {
                    //Debug.Log("Spawning target");
                    var target = (GameObject)Instantiate(TargetGameObject, spawnPosition, TargetGameObject.transform.rotation, Scene);
                    target.transform.LookAt(Elevator);
                    spawnFound = true;
                }
                else
                {
                    //Debug.Log("Failed to spawn object");
                }
            }
        }
        Debug.Log("Spawned " + numberOfSpawns + " targets");
        _level++;
    }

    /// <summary>
    /// Calculate the time you gain for finishing the current level
    /// </summary>
    /// <returns>A float representing the time you gained in seconds</returns>
    private float GetTimeForLevel()
    {
        return 20.0f;
    }

    private int GetTargetSpawnsForLevel()
    {
        if (_level <= 3)
        {
            return _level + 1;
        }
        else if (_level > 3)
        {
            return _rng.Next(3, 5);
        }
        else if (_level > 10)
        {
            return _rng.Next(3, 10);
        }
        else if (_level > 20)
        {
            return _rng.Next(10, 20);
        }
        else if (_level > 25)
        {
            return _rng.Next(15, 30);
        }
        else if (_level > 30)
        {
            return _rng.Next(25, 40);
        }
        else
        {
            return 40;
        }
    }

    private int GetElevatorSidesForLevel()
    {
        if (_level < 3)
        {
            return 1;
        }
        else if (_level == 3)
        {
            return 2;
        }
        else if (_level < 10)
        {
            return _rng.Next(1, 2);
        }
        else if (_level < 15)
        {
            return _rng.Next(1, 3);
        }
        else if (_level < 20)
        {
            return _rng.Next(2, 3);
        }
        else if (_level < 25)
        {
            return _rng.Next(3, 4);
        }
        else
        {
            return 4;
        }
    }

    public bool IsValidSpawn(Vector3 toSpawnPosition, Vector3 toSpawnBounds)
    {
        foreach (GameObject target in GameObject.FindGameObjectsWithTag("Target"))
        {
            Collider collider = target.GetComponentInChildren<Collider>(); //target.GetComponent<Collider>();
            if (IsWithinBounds(target.transform.position, FakeHalfBounds(collider.bounds), toSpawnPosition, toSpawnBounds))
            {
                return false;
            }
        }

        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Collider collider = obstacle.GetComponent<Collider>();
            if (IsWithinBounds(obstacle.transform.position, FakeHalfBounds(collider.bounds), toSpawnPosition, toSpawnBounds))
            {
                return false;
            }
        }

        return true;
    }

    public ElevatorDirection GetRandomDirection(ElevatorDirection[] directions)
    {
        return directions[_rng.Next(directions.Length)];
    }

    public Vector3 GetRandomPosition(ElevatorDirection direction)
    {
        // I think the best way of doing this would be to base it off randomizing a angle and then how far from
        // the center we should spawn the object. Then we figure what direction the object should be spawned.
        // When we know the distance and angle we multiply the angle with the offset for this direction and
        // finally calculate the x and y with x = distance * Math.cos(degrees) and y = distance * Math.sin(degrees)

        var degrees = _rng.NextDouble() * 80.0 + (double)direction - 45.0;
        //Debug.Log("Degrees: " + degrees);
        var radian =  degrees * (Math.PI / 180);
        var distance = 100;//MinDistance + _rng.NextDouble() * (MaxDistance - MinDistance);
        var x = distance * Math.Cos(radian);
        var z = distance * Math.Sin(radian);
        return new Vector3((float) x, 0.0f, (float) z);
    }

    public Vector3 FakeHalfBounds(Bounds bounds)
    {
        if (bounds.extents.x > bounds.extents.z)
        {
            return new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.x);
        }
        else
        {
            return new Vector3(bounds.extents.z, bounds.extents.y, bounds.extents.z);
        }
    }

    public bool IsWithinBounds(Vector3 p1, Vector3 b1, Vector3 p2, Vector3 b2)
    {
        float minXPos1 = p1.x - b1.x;
        float maxXPos1 = p1.x + b1.x;
        float minX2 = p2.x - b2.x;
        float maxX2 = p2.x + b2.x;
        if ((minXPos1 > minX2 && minXPos1 < maxX2) || (maxXPos1 < maxX2 && maxXPos1 > minX2))
        {
            return true;
        }
        float minZ1 = p1.z - b1.z;
        float maxZ1 = p1.z + b1.z;
        float minZ2 = p2.z - b2.z;
        float maxZ2 = p2.z + b2.z;
        if ((minZ1 > minZ2 && minZ1 < maxZ2) || (maxZ1 < maxZ2 && maxZ1 > minZ2))
        {
            return true;
        }
        return false;
    }

    public enum ElevatorDirection
    {
        North = 5,
        South = 95,
        West = 185,
        East = 275
    }
}
