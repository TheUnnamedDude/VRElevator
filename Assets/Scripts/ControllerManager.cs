using UnityEngine;

public class ControllerManager : MonoBehaviour {

    private SteamVR_TrackedObject _trackedObject;
    private SteamVR_Controller.Device device;

    public Rigidbody Bullet;
    public Transform BarrelOpening;
    public float Speed = 10;
    public float RecoilTime = 0.5F;
    private float _lastShot;

    public AudioClip Shot;
    public AudioClip Cock;
    private AudioSource _audio;

	// Use this for initialization
	void Start () {
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
        _audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        device = SteamVR_Controller.Input((int)_trackedObject.index);

		RaycastHit hit;

		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && _lastShot >= RecoilTime)
		{
		    var shotDirection = Vector3.forward; //BarrelOpening.transform.forward;
            _audio.PlayOneShot(Shot, 1f);
			
			if (Physics.Raycast(BarrelOpening.position, shotDirection, out hit))
			{
				TargetBehaviour targetScript = hit.collider.gameObject.GetComponent<TargetBehaviour>();
				if (targetScript != null)
				{
					targetScript.OnHit();
					Debug.DrawRay(BarrelOpening.position, hit.point, Color.green, 2.0f);
				}
			}

			Rigidbody bulletInstance = Instantiate(Bullet, BarrelOpening.position, BarrelOpening.rotation) as Rigidbody;
			bulletInstance.AddForce(shotDirection * 50000f * Speed);

			device.TriggerHapticPulse(3999);
			GetComponent<AudioSource>().PlayOneShot(Cock, 1f);
			_lastShot = 0;
		}
       
        if (_lastShot < RecoilTime)
            _lastShot += Time.deltaTime;
	}
}
