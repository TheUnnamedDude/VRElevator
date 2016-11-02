using System;
using UnityEngine;
using System.Linq;
using Zenject;
using Object = UnityEngine.Object;

public class GameController : IInitializable, ITickable
{
    public float BaseTimePerLevel;
    public double MinDistance = 40;
    public double MaxDistance = 130;

    [Inject]
    private ScoreManager _scoreManager;

    [Inject]
    private TargetBehaviour.Factory _targetFactory;

    private System.Random _rng;
    private int _seed;

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

    public void Initialize ()
    {
        IsRunning = true;
        InitializeRound();
    }

    public void Tick ()
    {
        if (_scoreManager.GameOver)
        {
              // TODO: Redo this
        }
    }

    public void InitializeRound()
    {
        _rng = new System.Random(Seed);
        FinishLevel();
    }

    public void OnTargetDestroy()
    {
        _scoreManager.AddTargetScore(10.0f); // TODO: Pass target type
        if (GetNumberOfTargetsAlive() <= 0)
        {
            FinishLevel();
        }
    }

    private int GetNumberOfTargetsAlive()
    {
        int targetsAlive = 0;
        foreach (var target in GameObject.FindGameObjectsWithTag("Target")) {
            TargetBehaviour targetScript = target.GetComponent<TargetBehaviour>();
            if (targetScript.Alive)
            {
                targetsAlive++;
            }
        }
        return targetsAlive;
    }

    public void FinishLevel()
    {
        // Make sure we don't have any game objects left
        foreach (var gObj in GameObject.FindGameObjectsWithTag("Target"))
        {
            Object.Destroy(gObj);
        }

        _scoreManager.NextLevel();

        var availableDirections = Enum.GetValues(typeof(ElevatorDirection))
            .Cast<ElevatorDirection>()
            .ToList();
        var directions = new ElevatorDirection[GetElevatorSidesForLevel()];
		Debug.Log("Starting level " + _scoreManager.Level);
        for (var i = 0; i < directions.Length; i++)
        {
            var directionIndex = _rng.Next(availableDirections.Count);
            directions[i] = availableDirections[directionIndex];
            availableDirections.RemoveAt(directionIndex);
        }

        var floorY = GameObject.FindGameObjectWithTag("Floor").transform.position.y;
        var numberOfSpawns = GetTargetSpawnsForLevel();
        for (var i = 0; i < numberOfSpawns; i++)
        {
            var spawnFound = false;
            for (var j = 0; j < 3  && !spawnFound; j++)
            {

                var direction = GetRandomDirection(directions);
                var rand = GetRandomPosition(direction);
                //if (!IsValidSpawn(spawnPosition, _targetGameObject.GetComponentInChildren<Collider>().bounds.extents))
                //    continue;
                var target = _targetFactory.CreateTarget(rand, floorY);
                spawnFound = true;
            }
        }
        Debug.Log("Spawned " + numberOfSpawns + " targets");
    }

    private int GetTargetSpawnsForLevel()
    {
        if (_scoreManager.Level <= 3)
        {
            return _scoreManager.Level + 1;
        }
        if (_scoreManager.Level < 10)
        {
            return _rng.Next(3, 5);
        }
        if (_scoreManager.Level < 20)
        {
            return _rng.Next(3, 10);
        }
        if (_scoreManager.Level < 25)
        {
            return _rng.Next(10, 20);
        }
        if (_scoreManager.Level < 30)
        {
            return _rng.Next(15, 30);
        }
        return _scoreManager.Level < 40 ? _rng.Next(25, 40) : 40;
    }

    private int GetElevatorSidesForLevel()
    {
        if (_scoreManager.Level < 3)
        {
            return 1;
        }
        if (_scoreManager.Level == 3)
        {
            return 2;
        }
        if (_scoreManager.Level < 10)
        {
            return _rng.Next(1, 2);
        }
        if (_scoreManager.Level < 15)
        {
            return _rng.Next(1, 3);
        }
        if (_scoreManager.Level < 20)
        {
            return _rng.Next(2, 3);
        }
        return _scoreManager.Level < 25 ? _rng.Next(3, 4) : 4;
    }

    public bool IsValidSpawn(Vector3 toSpawnPosition, Vector3 toSpawnBounds)
    {
        foreach (GameObject target in GameObject.FindGameObjectsWithTag("Target"))
        {
            var collider = target.GetComponentInChildren<Collider>(); //target.GetComponent<Collider>();
            if (IsWithinBounds(target.transform.position, FakeHalfBounds(collider.bounds), toSpawnPosition, toSpawnBounds))
            {
                return false;
            }
        }

        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            var collider = obstacle.GetComponent<Collider>();
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
        var distance = /*MaxDistance; /*/ MinDistance + _rng.NextDouble() * (MaxDistance - MinDistance);
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
        return new Vector3(bounds.extents.z, bounds.extents.y, bounds.extents.z);
    }

    public bool IsWithinBounds(Vector3 p1, Vector3 b1, Vector3 p2, Vector3 b2)
    {
        var minXPos1 = p1.x - b1.x;
        var maxXPos1 = p1.x + b1.x;
        var minX2 = p2.x - b2.x;
        var maxX2 = p2.x + b2.x;

        var minZ1 = p1.z - b1.z;
        var maxZ1 = p1.z + b1.z;
        var minZ2 = p2.z - b2.z;
        var maxZ2 = p2.z + b2.z;
        return (minXPos1 > minX2 && minXPos1 < maxX2) || (maxXPos1 < maxX2 && maxXPos1 > minX2)
               || (minZ1 > minZ2 && minZ1 < maxZ2) || (maxZ1 < maxZ2 && maxZ1 > minZ2);
    }

    public enum ElevatorDirection
    {
        North = 5,
        South = 95,
        West = 185,
        East = 275
    }
}
