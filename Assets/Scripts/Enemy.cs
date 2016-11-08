using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    [Inject]
    private GameController _gameController;
    private Renderer[] _renderers;
    private Rigidbody[] _rigidbodies;
    public float MaxHealth;
    public float AnimationTime;
    private float _animationElapsed;
    private bool _animationEnded;

    private float _health;

    public LevelGenerator.ElevatorDirection Direction;

    public bool Alive { get; private set; }

    public void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        ResetEnemy();
    }

    public void Update()
    {
        if (Alive || _animationEnded)
            return;

        _animationElapsed += Time.deltaTime;

        if (_animationElapsed <= AnimationTime)
            return;

        _animationEnded = true;
        ResetEnemy();
    }


    public void OnHit(float damage)
    {
        if (!Alive)
            return;
        _health -= damage;
        if (_health > 0)
            return;
        Alive = false;
        // Run animation then reset
        GetComponentInChildren<Animator>().SetTrigger("TriggerTargetFall");
        _gameController.OnTargetDestroy(20f);
    }

    public void ResetEnemy()
    {
        Alive = false;
        _health = MaxHealth;
        _animationEnded = false;
        _animationElapsed = 0;
        foreach (var rendr in _renderers)
        {
            rendr.enabled = false;
        }
        foreach (var rigd in _rigidbodies)
        {
            rigd.isKinematic = true;
        }
    }
    public void Show()
    {
        foreach (var rendr in _renderers)
        {
            rendr.enabled = true;
        }
        foreach (var rigd in _rigidbodies)
        {
            rigd.isKinematic = false;
        }
        Alive = true;
    }
}