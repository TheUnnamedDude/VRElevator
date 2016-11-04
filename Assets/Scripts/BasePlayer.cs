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
    public int FiringMode = 0;
    public int FullAmmo;
    public int TargetsHit = 0;
    public bool FullAuto;
    public int FiringCycle;

    public void UpdateRecoilTime()
    {
        if (_lastShot < RecoilTime)
            _lastShot += Time.deltaTime;
    }

    public bool ShootBullet()
    {
        if (CurrentAmmo >= 1)
        {
            if (_lastShot < RecoilTime)
                return false;

            var bullet = (Transform)Instantiate(Bullet, BarrelOpening.position, BarrelOpening.rotation);
            var bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.AddForce(BarrelOpening.forward * Speed);

            RaycastHit hit;
            if (Physics.Raycast(BarrelOpening.position, BarrelOpening.forward, out hit))
            {
                Debug.Log(hit.transform.gameObject);
                var targetScript = hit.collider.gameObject.GetComponentInParent<TargetBehaviour>();
                if (targetScript != null)
                {
                    Debug.Log("Hit a target");
                    targetScript.OnHit();
                    TargetsHit += 1;
                }
                else
                {
                    Debug.Log("Missed :/");
                }
                Debug.DrawRay(BarrelOpening.position, hit.point, Color.green, 2.0f);
            }
            _lastShot = 0;
            CurrentAmmo -= 1;
            return true;
        }
        else return false;

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

    public void SetValuesByFiringMode(int FiringMode)
    {
        if(FiringMode == 0)
        {
            FullAmmo = 6;
            RecoilTime = 0.1f;
            FullAuto = false;
            FiringCycle = 1;
        }
        if(FiringMode == 1)
        {
            FullAmmo = 30;
            RecoilTime = 0.1f;
            FullAuto = true;
        }
        if(FiringMode == 2)
        {
            FullAmmo = 30;
            RecoilTime = 0.05f;
            FiringCycle = 3;
            FullAuto = false;
        }
    }
}