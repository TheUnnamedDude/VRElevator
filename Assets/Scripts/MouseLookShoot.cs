using UnityEngine;
using System.Collections;

public class MouseLookShoot : MonoBehaviour {
    public Rigidbody bullet;
    public Transform barrelOpening;
    public float speed;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody bulletInstance;
            bulletInstance = Instantiate(bullet, barrelOpening.position, barrelOpening.rotation) as Rigidbody;
            bulletInstance.AddForce(barrelOpening.forward * speed);
        }
    }
}
