using UnityEngine;
using System.Collections;

public class MouseLookForTest : MonoBehaviour {

    public float sens = 5f;
    public float xRot;
    public float yRot;
    public float currentXRot;
    public float currentYRot;
    public float xRotVelocity;
    public float yRotVelocity;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        xRot -= Input.GetAxis("Mouse Y") * sens;
        yRot += Input.GetAxis("Mouse X") * sens;

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
    }
}
