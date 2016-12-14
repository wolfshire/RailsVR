using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUI : MonoBehaviour
{
    public float rotateSpeed = 100f;

    private RectTransform _transform;

    void Start()
    {
        _transform = GetComponent<RectTransform>();
    }

    void Update()
    {
        _transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
