using System.Collections;
using UnityEngine;

public class MouseLookShoot : BasePlayer {
    public float Sens = 2f;

    private float _xRot;
    private float _yRot;

	void Update () {
	    UpdateRecoilTime();
	    _xRot -= Input.GetAxis("Mouse Y") * Sens;
	    _yRot += Input.GetAxis("Mouse X") * Sens;
	    transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);

        if(currentEnergy < maxEnergy && !isFiring)
        {
            currentEnergy += ChargeSpeed * Time.deltaTime;
        }
        

        SetFiringMode();
        SetValuesByFiringMode(FiringMode);

        if(!FullAuto)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Burst());
            }
        }
        if(FullAuto)
        {
            if (Input.GetMouseButton(0))
            {
                isFiring = true;
                ShootBullet();
            }
            if(Input.GetMouseButtonUp(0))
            {
                isFiring = false;
            }
        }
	    
        if (Input.GetKeyDown(KeyCode.W))
        {
            previousFiringMode = true;
           
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            nextFiringMode = true;
        }
	}

    
}
