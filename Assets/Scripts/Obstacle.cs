using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Vector3 _targetPosition;
    private Vector3 _targetRotation;
    private const float MIN_Z = -11f;
    private const float MAX_Z = 11f;
    private const float MIN_X = -11f;
    private const float MAX_X = 11f;
    private const float Y_POS = 1.95f;
    [SerializeField] private float targetRadius = 0.1f;
    [SerializeField] private float _minTimeToSetTarget = 1f;
    [SerializeField] private float _maxTimeToSetTarget = 4f;
    private float _timeToSetTarget;
    private float _timer;
    private Coroutine movingCoroutine;

    private void Update()
    {
        if(_timeToSetTarget == 0)
        {
            _timeToSetTarget = Random.Range(_minTimeToSetTarget, _maxTimeToSetTarget);
            _timer = Time.time;
        }
            
        if (movingCoroutine == null)
        {
            if(Time.time - _timeToSetTarget > _timer)
            {
                movingCoroutine = StartCoroutine(MoveToDestination());
            }
        }
    }

    IEnumerator MoveToDestination()
    {
        SetTargetPosition();
        SetTargetRotation();

        bool reachedDestination = Vector3.Distance(transform.position, _targetPosition) <= targetRadius;
        while (!reachedDestination)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, 0.1f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_targetRotation), 0.1f);
            reachedDestination = Vector3.Distance(transform.position, _targetPosition) <= targetRadius;
            
            yield return null;
        }
        movingCoroutine = null;
        _timeToSetTarget = 0;
    }

    void SetTargetPosition()
    {
        float xPos = Random.Range(MIN_X, MAX_X);
        float zPos = Random.Range(MIN_Z, MAX_Z);
        _targetPosition = new Vector3(xPos, Y_POS, zPos);
    }

    void SetTargetRotation()
    {
        float yRot = Random.Range(0, 360);
        _targetRotation = new Vector3(0, yRot, 0);
    }
}
