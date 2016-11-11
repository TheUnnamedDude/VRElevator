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
        SetFiringMode();
        SetValuesByFiringMode(FiringMode);

        if (currentEnergy < maxEnergy && !isFiring)
        {
            currentEnergy += ChargeSpeed * Time.deltaTime;
        }

        UpdateRecoilTime();
	    device = SteamVR_Controller.Input((int)_trackedObject.index);

	    if (!FullAuto)
        {
            if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                StartCoroutine(Burst());
            }
        }
        if(FullAuto)
        {
            if(device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                isFiring = true;
                ShootBullet();
            }
            if(device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                isFiring = false;
            }
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x > 0.5f)
            {
                nextFiringMode = true;
            }
            if (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x < 0.5f)
            {
                previousFiringMode = true;
            }
            
        }

        _audio.PlayOneShot(Shot, 1f);
	    device.TriggerHapticPulse(3999);
	    GetComponent<AudioSource>().PlayOneShot(Cock, 1f);
	}
}
