using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {


    SteamVR_TrackedController controller;
    TargetScript target;

    // Use this for initialization
    void Start () {
        controller = GetComponent<SteamVR_TrackedController>();
        if(controller == null)
        {
            controller = gameObject.AddComponent<SteamVR_TrackedController>();
        }
        controller.TriggerClicked += new ClickedEventHandler(Fire);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Fire(object sender, ClickedEventArgs e)
    {
        Debug.Log("clicked fire");
        int layermask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward * 10, out hit, 10.0f, layermask))
        {
            target = hit.collider.gameObject.GetComponent<TargetScript>();
            target.SendMessage("HIT!");
            Debug.Log("Coderape hehe 8)");
        }
    }
}
