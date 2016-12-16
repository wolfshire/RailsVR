using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotateSpeed = 100f;

    private Transform _transform;

	void Start()
	{
        _transform = transform;
	}

	void Update()
	{
        _transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
	}
}
