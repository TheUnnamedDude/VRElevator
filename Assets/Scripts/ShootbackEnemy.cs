using UnityEngine;
using Zenject;

public class ShootbackEnemy : Enemy
{
    public float Timer;
    public GameObject Projectile;
    public AudioSource TargetLock;

    void Start()
    {
        LockOnTargetSound();
    }

    void Update()
    {
        base.Update();
        if (Alive)
        {
            TargetPlayer();
        }
    }

    private void LockOnTargetSound()
    {
        TargetLock.Play();
    }

    private void TargetPlayer()
    {
        Timer += Time.deltaTime;
        if(Timer >= 5f)
        {
            ShootProjectile();
            Timer = 0;
            LockOnTargetSound();
        }
    }
    private void ShootProjectile()
    {
        Instantiate(Projectile, gameObject.transform);
        Debug.Log("PROJECTILE ON ITS WAY!");
    }
}