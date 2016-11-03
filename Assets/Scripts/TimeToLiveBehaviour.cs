using UnityEngine;

public class TimeToLiveBehaviour : MonoBehaviour {
    public float TimeToLive = 10;

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
}
