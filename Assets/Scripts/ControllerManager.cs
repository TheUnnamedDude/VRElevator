using UnityEngine;
using Zenject;

public class ControllerManager : BasePlayer {

    private SteamVR_TrackedObject _trackedObject;
    private SteamVR_Controller.Device device;

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
	    UpdateRecoilTime();
	    device = SteamVR_Controller.Input((int)_trackedObject.index);

	    if (!device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) || !ShootBullet())
	        return;

	    _audio.PlayOneShot(Shot, 1f);
	    device.TriggerHapticPulse(3999);
	    GetComponent<AudioSource>().PlayOneShot(Cock, 1f);
	}
}
