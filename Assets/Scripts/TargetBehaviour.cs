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
        [Inject(Id = "Elevator")]
        private GameObject _elevator;

        [Inject(Id = "Scene")]
        private GameObject _scene;

        [Inject]
        private DiContainer _diContainer;

        public GameObject CreateTarget(Vector3 position, float floorY)
        {
            var spawnPosition = _elevator.transform.position + position;
            spawnPosition.y = floorY;
            var target = Create().gameObject;
            target.transform.parent = _scene.transform;
            target.transform.position = spawnPosition;
            target.transform.LookAt(_elevator.transform);
            return target;
        }
    }
}
