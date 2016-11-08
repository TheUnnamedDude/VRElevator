using UnityEngine;
using Zenject;

public class BasePlayer : MonoBehaviour
{
    private float _lastShot;
    public float Speed = 10;
    public Transform BarrelOpening;
    public Transform Bullet;

    public int CurrentAmmo;
    public float RecoilTime;
    public int FiringMode;
    public int FullAmmo;
    public int TargetsHit;
    public bool FullAuto;
    public int FiringCycle;

    public void UpdateRecoilTime()
    {
        if (_lastShot < RecoilTime)
            _lastShot += Time.deltaTime;
    }

    public bool ShootBullet()
    {
        if (CurrentAmmo < 1 || _lastShot < RecoilTime)
            return false;

        var bullet = (Transform)Instantiate(Bullet, BarrelOpening.position, BarrelOpening.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(BarrelOpening.forward * Speed);

        RaycastHit hit;
        if (Physics.Raycast(BarrelOpening.position, BarrelOpening.forward, out hit))
        {
            Debug.Log(hit.transform.gameObject);
            var enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Hit a enemy");
                enemy.OnHit(1f);
                TargetsHit += 1;
            }
            else
            {
                Debug.Log("Missed :/");
            }
            _lastShot = 0;
            CurrentAmmo -= 1;
            return true;
        }
        return false;

    }
    public bool Reload()
    {
        CurrentAmmo = FullAmmo;
        return true;
    }

    public void SetFiringMode()
    {
        if(TargetsHit < 3)
        {
            FiringMode = 0;
        }
        if(TargetsHit >= 3 && TargetsHit <= 6)
        {
            FiringMode = 1;
        }
        if(TargetsHit >6)
        {
            FiringMode = 2;
        }
        //return 0;
    }

    public void SetValuesByFiringMode(int firingMode)
    {
        if (firingMode == 0)
        {
            FullAmmo = 6;
            RecoilTime = 0.1f;
            FullAuto = false;
            FiringCycle = 1;
        }
        else if (firingMode == 1)
        {
            FullAmmo = 30;
            RecoilTime = 0.1f;
            FullAuto = true;
        }
        else if (firingMode == 2)
        {
            FullAmmo = 30;
            RecoilTime = 0.05f;
            FiringCycle = 3;
            FullAuto = false;
        }
    }
}