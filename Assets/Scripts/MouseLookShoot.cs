using UnityEngine;
using Zenject;

public class MouseLookShoot : BasePlayer {
    public float Sens = 2f;

    private float _xRot;
    private float _yRot;

	void Update () {
	    UpdateRecoilTime();
	    _xRot -= Input.GetAxis("Mouse Y") * Sens;
	    _yRot += Input.GetAxis("Mouse X") * Sens;
	    transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);

	    if (!Input.GetMouseButtonDown(0))
	        return;
	    ShootBullet();
	}
}
