using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringReticle : MonoBehaviour
{
    private SpriteRenderer _renderer;
    public bool Active { get; private set; }
    private float _elapsedTime;
    private float _endTime;
    private Camera _camera;
    private Transform _transform;
    private Transform _enemy;
    private float _heightOffset = 1.25f;

	void Start()
	{
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.enabled = false;
        Active = false;
        _camera = Camera.main;
        _transform = transform;
        _enemy = _transform.parent;
	}

	void Update()
	{
		if (Active)
        {
            _elapsedTime += Time.deltaTime;
            _elapsedTime = Mathf.Clamp(_elapsedTime, 0, _endTime);
            float percent = 1 - _elapsedTime / _endTime;
            _transform.localScale = new Vector3(percent, percent, 1);
        }
	}

    public void Begin(float timeRemaining)
    {
        _renderer.enabled = true;
        Active = true;
        _elapsedTime = 0;
        _endTime = timeRemaining;
        _transform.localScale.Set(1, 1, 1);

        _transform.position = new Vector3(_enemy.position.x, _enemy.position.y + _heightOffset, _enemy.position.z);
        _transform.LookAt(new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z));
        _transform.Translate(Vector3.forward * 2);
    }

    public void End()
    {
        _renderer.enabled = false;
        Active = false;
    }
}
