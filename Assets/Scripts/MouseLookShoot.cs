using UnityEngine;

public class MouseLookShoot : MonoBehaviour {
    public float Sens = 2f;
    public Rigidbody Bullet;
    public Transform BarrelOpening;
    public float Speed;

    private float _xRot;
    private float _yRot;

	void Update () {
	    _xRot -= Input.GetAxis("Mouse Y") * Sens;
	    _yRot += Input.GetAxis("Mouse X") * Sens;
	    transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);

	    if (!Input.GetMouseButtonDown(0))
	        return;

	    var shotDirection = BarrelOpening.transform.forward;
	    RaycastHit hit;
	    if (Physics.Raycast(BarrelOpening.position, shotDirection, out hit))
	    {
	        var target = hit.collider.gameObject.GetComponentInParent<TargetBehaviour>();
	        Debug.Log(hit.collider.gameObject);
	        if (target != null)
	        {
	            Debug.Log("Hit!");
	            target.OnHit();
	        }
	        Debug.DrawRay(BarrelOpening.position, hit.point, Color.green, 2.0f);
	    }
            
	    var bulletInstance = Instantiate(Bullet, BarrelOpening.position, BarrelOpening.rotation) as Rigidbody;
	    bulletInstance.AddForce(shotDirection * Speed);
	}
}
