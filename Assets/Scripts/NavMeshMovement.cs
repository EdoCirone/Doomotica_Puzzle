using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMovement : MonoBehaviour
{

    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponentInChildren<NavMeshAgent>();
    }

    public void MoveTo(Transform target)
    {
        if (_agent != null && target != null)
        {
            _agent.SetDestination(target.position);
        }
    }

    public void SetSpeed(float speed)
    {
        if (_agent != null)
        {
            _agent.speed = speed;
        }
    }

    public float GetSpeed()
    {
        return _agent != null ? _agent.speed : 2f;
    }

    public bool HasReachedDestination(float threshold = 0.01f)
    {
        if (_agent == null) return false;
        if (_agent.pathPending) return false;

        float dist = _agent.remainingDistance;
        float stop = Mathf.Max(_agent.stoppingDistance, threshold);

        return dist <= stop && (!_agent.hasPath || _agent.velocity.sqrMagnitude < 0.01f);
    }

    public void Stop()
    {
        if (_agent != null)
        {

            _agent.ResetPath();
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;

        }
    }


}


