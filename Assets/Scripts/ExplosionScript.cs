using UnityEngine;
using System.Collections;

<<<<<<< HEAD
public class ExplosionScript : MonoBehaviour
{
=======
public class ExplosionScript : MonoBehaviour {
>>>>>>> a5d557ad2d3ed39cbc5d6415b774080785cecc0c
    public float TimeToLive = 0.3f;

    private float _timeLived;
    // Use this for initialization
<<<<<<< HEAD
    void Start()
    {

    }
=======
    void Start () {
>>>>>>> a5d557ad2d3ed39cbc5d6415b774080785cecc0c

    // Update is called once per frame
    void Update()
    {
        _timeLived += Time.deltaTime;

        if (_timeLived > TimeToLive)
        {
            Destroy(gameObject);
        }
    }
<<<<<<< HEAD
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponentInParent<Enemy>())
        {
            var enemy = collision.collider.gameObject.GetComponentInParent<Enemy>();
            enemy.OnHit(3f);
        }

=======
	
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
        
>>>>>>> a5d557ad2d3ed39cbc5d6415b774080785cecc0c
    }
}
