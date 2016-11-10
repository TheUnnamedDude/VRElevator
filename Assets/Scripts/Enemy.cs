using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    [Inject]
    private GameController _gameController;
    private Renderer[] _renderers;
    public float MaxHealth;
    public float AnimationTime;
    private float _animationElapsed;
    private bool _animationEnded;
    private int _defaultLayer;
    private int _ignoreRaycast;

    private float _health;

    public LevelGenerator.ElevatorDirection Direction;

    public bool Alive { get; private set; }

    public void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _defaultLayer = gameObject.layer;
        _ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
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
        gameObject.layer = _ignoreRaycast;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = _ignoreRaycast;
        }
    }
    public void Show()
    {
        Debug.Log("Showing");
        foreach (var rendr in _renderers)
        {
            Debug.Log(rendr);
            rendr.enabled = true;
        }
        Alive = true;
        gameObject.layer = _defaultLayer;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = _defaultLayer;
        }
    }
}