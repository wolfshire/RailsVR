using UnityEngine;

public class AccelerationMove : MonoBehaviour
{
    [Range(1f, 10f)]
    public float MaxSpeed = 1;
    public float MinSpeed = 0.5f;
    public float AccelerationPercent = 0.1f;
    
    public bool Arrived { get { return !_moving; } }

    private Transform _transform;
    private Vector3 _startPos;
    private Transform[] _targetNodes;
    private bool _moving;

    private int destinationIndex;
    private float _speed;
    private float[] _distances;
    private float _totalDistance;
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
            // Move towards the next node
            _transform.position = Vector3.MoveTowards(_transform.position, _targetNodes[destinationIndex].position, _speed * Time.deltaTime);

            Vector3 distanceVector = _targetNodes[destinationIndex].position - _transform.position;
            float distance = distanceVector.magnitude;

            if (destinationIndex == 0)
            {
                Vector3 fromStart = _transform.position - _startPos;
                float fromStartDistance = fromStart.magnitude;

                if (fromStartDistance < _accDistance)
                {
                    _speed = MaxSpeed * (fromStartDistance / _accDistance);
                    _speed = Mathf.Clamp(_speed, MinSpeed, MaxSpeed);
                }
            }
            else if (destinationIndex == _targetNodes.Length - 1 && distance < _decDistance)
            {
                _speed = MaxSpeed * (distance / _decDistance);
            }
            else
                _speed = MaxSpeed;

            if (distance < 0.025f)
            {
                if (destinationIndex == _targetNodes.Length - 1)
                {
                    _moving = false;
                    _transform.position = _targetNodes[destinationIndex].position;
                }
                else
                {
                    destinationIndex++;
                }
            }
        }
    }

    public void StartMove(Transform[] nodes)
    {
        if (nodes == null) return;

        _targetNodes = nodes;
        _distances = new float[_targetNodes.Length - 1];

        if (_distances.Length == 0)
        {
            _totalDistance = (_targetNodes[0].position - _transform.position).magnitude;
            _distances = new float[] { _totalDistance };
        }
        else
        {
            for (int i = 0; i < _distances.Length; i++)
            {
                _distances[i] = (_targetNodes[i + 1].position - _targetNodes[i].position).magnitude;
                _totalDistance += _distances[i];
            }
        }

        _accDistance = _distances[0] * AccelerationPercent;
        _decDistance = _distances[_distances.Length - 1] * AccelerationPercent;

        destinationIndex = 0;
        _startPos = _transform.position;
        _moving = true;
    }
}
