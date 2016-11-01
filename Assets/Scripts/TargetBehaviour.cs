using UnityEngine;
using Zenject;

public class TargetBehaviour : MonoBehaviour {
    public AudioClip TargetHit;
    private AudioSource _audio;

    [Inject]
    private GameController _controller;
    private bool _alive = true;

    public bool Alive { get { return _alive; } }

    void Start () {
        _audio = GetComponent<AudioSource>();
    }

    public void OnHit()
    {
        if (!Alive)
            return;
        _alive = false;
        _audio.PlayOneShot(TargetHit, 1f);
        GetComponentInChildren<Animator>().SetTrigger("TriggerTargetFall");
        Destroy(gameObject, 2);
        _controller.OnTargetDestroy();
    }

    public class Factory : Factory<TargetBehaviour>
    {

    }
}
