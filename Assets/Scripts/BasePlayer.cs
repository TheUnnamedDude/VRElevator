using UnityEngine;
using Zenject;

public class BasePlayer : MonoBehaviour
{
    private float _lastShot;
    public float Speed = 10;
    public Transform BarrelOpening;
    public Transform Bullet;
    public Transform ExplosionSphere;

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

        CurrentAmmo -= 1;
        var bullet = (Transform)Instantiate(Bullet, BarrelOpening.position, BarrelOpening.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(BarrelOpening.forward * Speed);

        RaycastHit hit;
        if (Physics.Raycast(BarrelOpening.position, BarrelOpening.forward, out hit))
        {
            Debug.Log(hit.transform.gameObject);
            
            var enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();
            if(FiringMode != 3)
            {
                if (enemy != null)
                {
                    Debug.Log("Hit a enemy");
                    enemy.OnHit(1f);
                    //TargetsHit += 1;
                }
                else
                {
                    Debug.Log("Missed :/");
                }
            }
            else if (FiringMode == 3)
            {
                Instantiate(ExplosionSphere, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
            }
            
            
            _lastShot = 0;
            
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
        if(Input.GetKeyDown(KeyCode.E) && (FiringMode < 3))
        {
            FiringMode++;
        }
        if(Input.GetKeyDown(KeyCode.W) && (FiringMode > 0) )
        {
            FiringMode--;
        }
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
        if(FiringMode == 3)
        {
            FullAmmo = 5;
            RecoilTime = 0.5f;
            FiringCycle = 1;
            FullAuto = false;
        }
    }
}