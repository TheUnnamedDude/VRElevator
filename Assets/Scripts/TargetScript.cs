using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {

    GameController _controller;
    public AudioClip TargetHit;
    private new AudioSource _audio;

    public AnimationClip AnimationClip;
    private bool _alive = true;

    public bool Alive { get { return _alive; }
    }

    // Use this for initialization
    void Start () {
        _audio = GetComponent<AudioSource>();
        _controller = (GameController) FindObjectOfType(typeof(GameController));
        
    }
	
	// Update is called once per frame
	void Update () {
	}

    /*void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ammunition")
        {

        }
        
    }*/

    public void Hit()
    {
        //_audio.PlayOneShot(TargetHit, 1f);
        var animator = GetComponent<Animator>();
        animator.SetTrigger("TriggerTargetFall");
        _alive = false;
        
        Destroy(transform.parent.gameObject, AnimationClip.length/2);
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
