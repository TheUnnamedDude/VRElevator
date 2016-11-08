using UnityEngine;
using Zenject;

public class ProjectileFromShootbackTarget : Enemy
{
    private float speed = 25;
    private Vector3 TargetPath;
    public AudioSource LoopingProjectileSound;

    void Start()
    {
        LoopingProjectileSound.Play();
        TargetPath = GameObject.FindGameObjectWithTag("Player").transform.position;
        Time.timeScale = 0.50f;
    }

    void Update()
    {
        base.Update();
        Shoot();
    }

    private void Shoot()
    {
        float step = speed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, TargetPath, step);
        if (gameObject.transform.position == TargetPath) //TODO: Add logic for if hit targetPath
        {
            Time.timeScale = 1f;
            //Damage player or reduce time left on stage
        }
    }
}
