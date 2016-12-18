using UnityEngine;

public class AccelerationMove : MonoBehaviour
{
    [Range(1f, 10f)]
    public float MaxSpeed = 1;
    public float MinSpeed = 0.5f;
    public float AccelerationPercent = 0.1f;
    

    public bool Arrived { get { return !_moving; } }

    private Transform _transform;
    private Transform _targetTransform;
    private bool _moving;

    private float _speed;
    private float _startDistance;
    private float _accDistance;
    private float _decDistance;

	void Awake()
	{
        _transform = transform;
        _moving = false;
    }

	void Update()
	{
        if (_moving)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _targetTransform.position, _speed * Time.deltaTime);
            Vector3 distanceVector = _targetTransform.position - _transform.position;
            float distance = distanceVector.magnitude;

            if (distance > _accDistance)
            {
                _speed = MaxSpeed * ((_startDistance - distance) / _decDistance);
                _speed = Mathf.Clamp(_speed, MinSpeed, MaxSpeed);
            }
            else if (distance < _decDistance)
            {
                _speed = MaxSpeed * (distance / _decDistance);
            }
            else
                _speed = MaxSpeed;

            if (distance < 0.025f)
            {
                _moving = false;
                _transform.position = _targetTransform.position;
            }

            
        }
    }

    public void StartMove(Transform target)
    {
        _targetTransform = target;

        if (_targetTransform != null)
        {
            _moving = true;

            _startDistance = (_targetTransform.position - _transform.position).magnitude;
            _decDistance = _startDistance * AccelerationPercent;
            _accDistance = _startDistance - _decDistance;
        }
    }
}
