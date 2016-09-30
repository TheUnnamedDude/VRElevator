using UnityEngine;
using System.Collections;

public class ControllerManager : MonoBehaviour {

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    public Rigidbody bullet;
    public Transform barrelOpening;
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

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && _lastShot >= _recoilTime)
        {
            audio.PlayOneShot(Shot, 1f);
            Rigidbody bulletInstance;
            bulletInstance = Instantiate(bullet, barrelOpening.position, barrelOpening.rotation) as Rigidbody;
            bulletInstance.AddForce(barrelOpening.forward * 10000 * Speed);
            //while (true)
            device.TriggerHapticPulse(3999);
            audio.PlayOneShot(Cock, 1f);
            //bulletInstance.rotation.SetFromToRotation();
            _lastShot = 0;
        }
        if (_lastShot < _recoilTime)
            _lastShot += Time.deltaTime;
	}
}
