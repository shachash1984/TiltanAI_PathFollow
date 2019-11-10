using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowing : MonoBehaviour
{
    public CreatePath path;
    public float speed = 5.0f;
    public float mass = 5.0f;
    public bool isLooping = true;

    private float _curSpeed;
    private int _curPathIndex;
    private int _pathLength;
    private Vector3 _targetPoint;
    private Vector3 _velocity;
    public bool isReversed = false;

    private void Start()
    {
        if (!path)
            path = FindObjectOfType<CreatePath>();
        _pathLength = path.Length;
        if (!isReversed)
            _curPathIndex = 0;
        else
            _curPathIndex = _pathLength - 1;
        _velocity = transform.forward;
    }

    private void Update()
    {
        _curSpeed = speed * Time.deltaTime;
        _targetPoint = path.GetPoint(_curPathIndex);

        if (Vector3.Distance(transform.position, _targetPoint) < path.Radius)
        {
            if(!isReversed)
            {
                if (_curPathIndex < _pathLength - 1)
                {
                    _curPathIndex++;
                }
                else if (isLooping)
                {
                    _curPathIndex = 0;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (_curPathIndex > 0)
                {
                    _curPathIndex--;
                }
                else if (isLooping)
                {
                    _curPathIndex = _pathLength - 1;
                }
                else
                {
                    return;
                }
            }
            
        }

        if (_curPathIndex >= _pathLength && !isReversed)
        {
            return;
        }
        else if(_curPathIndex <= 0 && isReversed)
        {
            return;
        }

        if(!isReversed)
        {
            if (_curPathIndex >= _pathLength - 1 && !isLooping)
            {
                _velocity += Steer(_targetPoint, true);
            }
            else
            {
                _velocity += Steer(_targetPoint);
            }
        }
        else
        {
            if (_curPathIndex <= 0 && !isLooping)
            {
                _velocity += Steer(_targetPoint, true);
            }
            else
            {
                _velocity += Steer(_targetPoint);
            }
        }
        

        transform.position += _velocity;
        transform.rotation = Quaternion.LookRotation(_velocity);
    }

    public Vector3 Steer(Vector3 target, bool bFinalPoint = false)
    {
        Vector3 desired_velocity = (target - transform.position);
        float dist = desired_velocity.magnitude;
        desired_velocity.Normalize();

        if (bFinalPoint && dist < 10.0f)
        {
            desired_velocity *= (_curSpeed * (dist / 10.0f));
        }
        else
        {
            desired_velocity *= _curSpeed;
        }

        Vector3 steeringForce = desired_velocity - _velocity;
        Vector3 acceleration = steeringForce / mass;
        return acceleration;
    }
}

