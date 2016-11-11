using System.Collections;
using UnityEngine;
using Zenject;

public class BasePlayer : MonoBehaviour
{
    
    public float Speed = 10;
    public Transform BarrelOpening;
    public Transform Bullet;
    public Transform ExplosionSphere;

    //firin mode variables
    private float _lastShot;
    public float RecoilTime;
    public int FiringMode;
    public bool FullAuto;
    public int FiringCycle;
    public float Damage;

    //Energy management variables
    public float maxEnergy = 100f;
    public float ChargeSpeed = 20f;
    public float currentEnergy;
    public float energyDecrease;
    public bool isFiring = false;

    public bool nextFiringMode = false;
    public bool previousFiringMode = false;

    void Start()
    {
        currentEnergy = maxEnergy;
    }
    

    public void UpdateRecoilTime()
    {
        if (_lastShot < RecoilTime)
            _lastShot += Time.deltaTime;
    }

    public bool ShootBullet()
    {
        if (currentEnergy < energyDecrease || _lastShot < RecoilTime)
            return false;

        currentEnergy -= energyDecrease;

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
                    Debug.Log("Hit an enemy");
                    enemy.OnHit(Damage);
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

    /*public bool Reload()
    {
        if(currentEnergy < maxEnergy)
        {
            
        }
        
        //CurrentAmmo = FullAmmo;
        return true;
    }*/

    public void SetFiringMode()
    {
        if(nextFiringMode && (FiringMode < 3))
        {
            FiringMode++;
            nextFiringMode = false;
        }
        if(previousFiringMode && (FiringMode > 0) )
        {
            FiringMode--;
            previousFiringMode = false;
        }
    }

    public void SetValuesByFiringMode(int firingMode)
    {
        if (firingMode == 0)
        {
            energyDecrease = 16.66f;
            RecoilTime = 0.1f;
            FullAuto = false;
            FiringCycle = 1;
            Damage = 2f;
        }
        else if (firingMode == 1)
        {
            energyDecrease = 3f;
            RecoilTime = 0.1f;
            FullAuto = true;
            Damage = 1f;
        }
        else if (firingMode == 2)
        {
            energyDecrease = 6f;
            RecoilTime = 0.05f;
            FiringCycle = 3;
            FullAuto = false;
            Damage = 1.5f;
        }
        if(FiringMode == 3)
        {
            energyDecrease = 50f;
            RecoilTime = 0.5f;
            FiringCycle = 1;
            FullAuto = false;
            Damage = 3f;
        }
    }
    //inspirert av https://forum.unity3d.com/threads/help-with-burst-fire-script-solved.38040/
    public IEnumerator Burst()
    {
        if(!FullAuto)
        {
            if(energyDecrease < currentEnergy)
            {
                isFiring = true;
                for (int i = 0; i < FiringCycle; i++)
                {
                    ShootBullet();
                    yield return new WaitForSeconds(RecoilTime);
                }
                isFiring = false;
            }
            
        }
        /*if(FullAuto)
        {
            ShootBullet();
            yield return new WaitForSeconds(RecoilTime);
        }*/
    }
}