using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {
    public float TimeToLive = 0.3f;

    private float _timeLived;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        _timeLived += Time.deltaTime;

        if (_timeLived > TimeToLive)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.GetComponentInParent<Enemy>())
        {
            var enemy = collision.collider.gameObject.GetComponentInParent<Enemy>();
            enemy.OnHit(1f);
        }
        
    }
}
