using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform Elevator;
    public GameObject TargetGameObject;
    public float BaseTimePerLevel;
    public double MinDistance;
    public double MaxDistance;

    private System.Random _rng;
    private int _seed;

    private int _level = 1;
    private float _timeElapsed;
    private float _timeLimit;
    private int _score;

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

    }

    public void InitializeRound()
    {
        _rng = new System.Random(Seed);
        FinishLevel();
    }

    public void OnTargetDestroy()
    {
        Debug.Log(GameObject.FindGameObjectsWithTag("Target").Length);

        if (GameObject.FindGameObjectsWithTag("Target").Length <= 1) //Off by 1 so default set to 1 so we can destroy the object after running this method
        {
            FinishLevel();
        }
    }

    public void FinishLevel()
    {
        // Make sure we don't have any game objects left
        foreach (var gObj in GameObject.FindGameObjectsWithTag("Target"))
        {
            Destroy(gObj);
        }

        Debug.Log("Starting spawn");
        List<ElevatorDirection> availableDirections = Enum.GetValues(typeof(ElevatorDirection))
            .Cast<ElevatorDirection>()
            .ToList();
        var directions = new ElevatorDirection[GetElevatorSidesForLevel()];
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.Log("Spawning targets at " + directions[i]);
            var directionIndex = _rng.Next(directions.Length);
            directions[i] = availableDirections[directionIndex];
            availableDirections.RemoveAt(directionIndex);
        }

        _timeLimit += GetTimeForLevel();

        float floorY = GameObject.FindGameObjectWithTag("Floor").transform.position.y;
        int numberOfSpawns = GetTargetSpawnsForLevel();
        for (int i = 0; i < numberOfSpawns; i++)
        {
            bool spawnFound = false;
            for (int j = 0; j < 3 && !spawnFound; j++)
            {
                // TODO: Floor position
                Vector3 spawnPosition = GetRandomPosition(directions);
                if (IsValidSpawn(spawnPosition, TargetGameObject.GetComponent<Collider>().bounds.extents))
                {
                    Debug.Log("Spawning target");
                    var target = (GameObject) Instantiate(TargetGameObject, spawnPosition, Quaternion.identity);
                    spawnFound = true;
                }
                else
                {
                    Debug.Log("Failed to spawn object");
                }
            }
        }
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
        if (_level == 1)
        {
            return 2;
        }
        else if (_level == 2)
        {
            return 3;
        }
        else if (_level == 3)
        {
            return 3;
        }
        if (_level > 3)
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
            Collider collider = target.GetComponent<Collider>();
            if (IsWithinBounds(target.transform.position, FakeHalfBounds(collider.bounds), toSpawnPosition, toSpawnBounds))
            {
                Debug.Log(target.transform.position + " would collide with " + toSpawnPosition);
                return false;
            }
        }

        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Collider collider = obstacle.GetComponent<Collider>();
            if (IsWithinBounds(obstacle.transform.position, FakeHalfBounds(collider.bounds), toSpawnPosition, toSpawnBounds))
            {
                Debug.Log(obstacle.transform.position + " would collide with " + toSpawnPosition);
                return false;
            }
        }

        return true;
    }

    public Vector3 GetRandomPosition(ElevatorDirection[] directions)
    {
        // I think the best way of doing this would be to base it off randomizing a angle and then how far from
        // the center we should spawn the object. Then we figure what direction the object should be spawned.
        // When we know the distance and angle we multiply the angle with the offset for this direction and
        // finally calculate the x and y with x = distance * Math.cos(degrees) and y = distance * Math.sin(degrees)

        var direction = directions[_rng.Next(directions.Length)];
        var degrees = _rng.NextDouble() * 80.0 + (int) direction;
        var distance = MinDistance + _rng.NextDouble() * (MaxDistance - MinDistance);
        var x = distance * Math.Cos(degrees);
        var y = distance * Math.Sin(degrees);
        return new Vector3((float) x, 0.0f, (float) y);
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
