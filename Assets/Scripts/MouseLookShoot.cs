using UnityEngine;
using System.Collections;

public class MouseLookShoot : MonoBehaviour {
    public Rigidbody Bullet;
    public Transform BarrelOpening;
    public float speed;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Vector3 ShotDirection = BarrelOpening.transform.forward;
        Ray hitScan = new Ray(BarrelOpening.position, ShotDirection);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(BarrelOpening.position, ShotDirection, out hit))
            {
                TargetScript targetScript = hit.collider.gameObject.GetComponent<TargetScript>();
                if (targetScript != null)
                { 
                    targetScript.Hit();
                    Debug.DrawRay(BarrelOpening.position, hit.point, Color.green, 2.0f);
                }
            }
            
            Rigidbody bulletInstance;
            bulletInstance = Instantiate(Bullet, BarrelOpening.position, BarrelOpening.rotation) as Rigidbody;
            bulletInstance.AddForce(ShotDirection * 20000f);
        }
        
    }
}
