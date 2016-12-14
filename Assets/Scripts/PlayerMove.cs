using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    [Range(1f, 10f)]
    public float Speed = 1;

    public bool Arrived { get { return !_moving; } }
    
    private Transform _targetTransform;
    private bool _moving;

	void Start()
	{
        _moving = false;
    }

	void Update()
	{
        if (_moving)
        {
            Vector3 distance = _targetTransform.position - transform.position;
            transform.position += distance.normalized * Time.deltaTime * Speed;

            if (distance.magnitude < Speed * Time.deltaTime)
            {
                _moving = false;
                transform.position = _targetTransform.position;
            }
        }
    }

    public void Move(Transform target)
    {
        Debug.Log("Moving");

        if (!_moving)
        {

            _targetTransform = target;

            if (_targetTransform != null)
                _moving = true;
        }
    }
}
