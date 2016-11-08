using UnityEngine;

public class MovingEnemy : Enemy
{
    public float speed = 25;
    private Vector3 _path;
    public bool OnTheMove;
    public GameObject DestinationPath;
    private Vector3 _startPosition;
    
    void Start()
    {
        _startPosition = gameObject.transform.position;
        _path = DestinationPath.transform.position;
    }

    void Update()
    {
        base.Update();
        if (Alive) {
            Movement();
        }
    }

    private void Movement()
    {
        var step = speed * Time.deltaTime;
        if (!OnTheMove)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _startPosition, step);
            if (gameObject.transform.position == _startPosition)
            {
                OnTheMove = true;
            }
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _path, step);
            if (gameObject.transform.position == _path)
            {
                OnTheMove = false;
            }
        }
    }
}