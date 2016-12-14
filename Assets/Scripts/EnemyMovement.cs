using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform _destination;

    public void SetDestination(Transform destination)
    {
        _destination = destination;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.SetDestination(_destination.position);
    }
}
