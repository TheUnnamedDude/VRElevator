using System.Collections;
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

        SetFiringMode();
        SetValuesByFiringMode(FiringMode);

        if(!FullAuto)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //inspirert av https://forum.unity3d.com/threads/help-with-burst-fire-script-solved.38040/
                StartCoroutine(Shoot());
            }
        }
        if(FullAuto)
        {
            if (Input.GetMouseButton(0))
            {
                ShootBullet();
            }
        }
	    
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
            
        
	}
    //inspirert av https://forum.unity3d.com/threads/help-with-burst-fire-script-solved.38040/
    IEnumerator Shoot()
    {
        for(int i = 0; i < FiringCycle; i++)
        {
            ShootBullet();
            yield return new WaitForSeconds(RecoilTime);
        }
    }
}
