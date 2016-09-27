using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {

    GameController _controller;
    

    // Use this for initialization
    void Start () {
        _controller = (GameController) FindObjectOfType(typeof(GameController));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Target hit!");
        Destroy(gameObject);
        _controller.OnTargetDestroy();
        
    }



    //void Hit()
    //{
    //    GetComponent<MeshRenderer>().enabled = false;
    //    foreach (ParticleSystem _system in gameObject.GetComponentsInChildren<ParticleSystem>(true))
    //    {
    //        _system.Play();
    //    }
    //    _controller.SendMessage("New target");
    //    Destroy(gameObject);
    //}
}
