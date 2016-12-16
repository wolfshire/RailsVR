using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Shootable
{
    private Transform _transform;
    private Move _moveController;
    private Health _health;
    private Transform _playerTransform;
    private Transform _playerHead;
    private FiringReticle _reticle;
    private bool _aiming;
    private float _shotTimer;
    [SerializeField]private float _attackSpeed = 3;

    private AudioSource _audioSource;
    public AudioClip[] audioFiles;


    private Ray _ray = new Ray();
    private RaycastHit _hit;

    void Start()
    {
        _transform = transform;
        _moveController = GetComponent<Move>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerHead = _playerTransform.transform.Find("Camera (eye)").transform;
        _audioSource = GetComponent<AudioSource>();
        _shotTimer = Random.value;
        _reticle = GetComponentInChildren<FiringReticle>();

        _health = GetComponent<Health>();
        _health.Death += () =>
        {
            _reticle.End();
            this.enabled = false;
        };
    }

	void Update()
	{
        if (_moveController.Arrived)
        {
            _aiming = true;
        }
        if (_aiming)
        {
            _shotTimer += Time.deltaTime;
            if (!_reticle.Active && _shotTimer > _attackSpeed * 0.5)
            {
                _reticle.Begin(_attackSpeed - _shotTimer);
            }
            if (_shotTimer > _attackSpeed)
            {
                _reticle.End();
                Shoot();
            }
        }
	}

    private void Shoot()
    {
        _ray.origin = _transform.position;
        _ray.direction = _playerHead.position - _transform.position;

        _audioSource.PlayOneShot(audioFiles[0], .5f);

        if (Physics.Raycast(_ray, out _hit, 100))
        {
            if(_hit.transform.gameObject.name == "Camera (eye)")
            {
                _playerTransform.GetComponent<Health>().TakeDamage(1);
            }
        }
        _shotTimer = Random.value;
    }

    public override void OnClick()
    {
        if (enabled)
        {
            _audioSource.PlayOneShot(audioFiles[1], 1f);
            _health.TakeDamage(1);
        }
    }
}
