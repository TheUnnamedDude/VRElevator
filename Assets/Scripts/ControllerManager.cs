using UnityEngine;
using System.Collections;

public class ControllerManager : MonoBehaviour {

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    public Rigidbody Bullet;
    public Transform BarrelOpening;
    public float Speed = 10;
    public float _recoilTime = 0.5F;
    private float _lastShot = 0f;

    public AudioClip Shot;
    public AudioClip Cock;
    private new AudioSource audio;

	// Use this for initialization
	void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        device = SteamVR_Controller.Input((int)trackedObject.index);

		RaycastHit hit;
		Vector3 ShotDirection = BarrelOpening.transform.forward;

		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && _lastShot >= _recoilTime)
        {
            audio.PlayOneShot(Shot, 1f);
			
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
			bulletInstance.AddForce(ShotDirection * 50000f * Speed);

			device.TriggerHapticPulse(3999);
			audio.PlayOneShot(Cock, 1f);
			_lastShot = 0;
		}
       
        if (_lastShot < _recoilTime)
            _lastShot += Time.deltaTime;
	}
}
