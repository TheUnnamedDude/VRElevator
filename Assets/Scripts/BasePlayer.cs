using UnityEngine;
using Zenject;

public class BasePlayer : MonoBehaviour
{
    private float _lastShot;
    public float Speed = 10;
    public float RecoilTime = 0.5F;
    public Transform BarrelOpening;
    public Transform Bullet;

    public void UpdateRecoilTime()
    {
        if (_lastShot < RecoilTime)
            _lastShot += Time.deltaTime;
    }

    public bool ShootBullet()
    {
        if (_lastShot < RecoilTime)
            return false;

        var bullet = (Transform) Instantiate(Bullet, BarrelOpening.position, BarrelOpening.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(BarrelOpening.forward * Speed);

        //Debug.DrawLine(BarrelOpening.position, BarrelOpening.position + BarrelOpening.forward * 10, Color.green, 2.0f);

        RaycastHit hit;
        if (Physics.Raycast(BarrelOpening.position, BarrelOpening.forward, out hit))
        {
            Debug.Log(hit.transform.gameObject);
            var targetScript = hit.collider.gameObject.GetComponentInParent<TargetBehaviour>();
            if (targetScript != null)
            {
                Debug.Log("Hit a target");
                targetScript.OnHit();
            }
            Debug.DrawRay(BarrelOpening.position, hit.point, Color.green, 2.0f);
        }
        _lastShot = 0;
        return true;
    }
}