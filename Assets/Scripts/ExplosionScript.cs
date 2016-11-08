using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.collider.gameObject.GetComponentInParent<Enemy>();
        enemy.OnHit(1f);
    }
}
