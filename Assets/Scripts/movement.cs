using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {
    public float speed;
    public Vector3 movement2;
    public Vector3 currentPlace;
    public float timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        currentPlace = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        movement2 = new Vector3(0, transform.position.y + speed, 0);

        if (transform.position.y >= 19)
        {
            timer += 1;
        } else if (transform.position.y >= 39)
        {
            timer += 1;
        } else
        {

        }
        
        if (timer <= 100)
        {
            transform.position = currentPlace;
        }
        else
        {
            transform.position = movement2;
        }

    }
}
