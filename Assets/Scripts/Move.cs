using UnityEngine;

public class Move : MonoBehaviour
{
    [Range(1f, 10f)]
    public float Speed = 1;

    public bool Arrived { get { return !_moving; } }

    private Transform _transform;
    private Transform _targetTransform;
    private bool _moving;

	void Awake()
	{
        _transform = transform;
        _moving = false;
    }

	void Update()
	{
        if (_moving)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _targetTransform.position, Speed * Time.deltaTime);
            Vector3 distance = _targetTransform.position - _transform.position;
            if (distance.magnitude < 0.05f)
            {
                _moving = false;
                _transform.position = _targetTransform.position;
            }
        }
    }

    public void StartMove(Transform target)
    {
        Debug.Log("Moving");

        _targetTransform = target;

        if (_targetTransform != null)
        {
            _moving = true;
        }
    }
}
