using UnityEngine;
using System.Collections;

public class ControllerManager : MonoBehaviour {

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    public Rigidbody bullet;
    public Transform barrelOpening;
    public int Speed = 12000;

	// Use this for initialization
	void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
        device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("PEWPEW");
            Rigidbody bulletInstance;
            bulletInstance = Instantiate(bullet, barrelOpening.position, barrelOpening.rotation) as Rigidbody;
            bulletInstance.AddForce(barrelOpening.forward * Speed);
        }
	}
}
