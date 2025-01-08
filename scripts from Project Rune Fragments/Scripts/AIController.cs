using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public float movementRadius;
    public Transform anchorPoint;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        GlobalEnemyEvents.Instance.OnMovementSpeedChanged += UpdateSpeed;
        UpdateSpeed(GlobalEnemyEvents.Instance.GetCurrentMovementSpeed());
    }

    void Update()
    {
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            Vector3 targetPos;
            if (FindRandomPosition(anchorPoint.position, movementRadius, out targetPos))
            {
                // Debug.DrawRay(targetPos, Vector3.up, Color.red, 1.0f);
                navAgent.SetDestination(targetPos);
            }
        }
    }

    bool FindRandomPosition(Vector3 origin, float radius, out Vector3 navPoint)
    {
        Vector3 potentialPos = origin + Random.insideUnitSphere * radius;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(potentialPos, out navHit, 1.0f, NavMesh.AllAreas))
        {
            navPoint = navHit.position;
            return true;
        }

        navPoint = Vector3.zero;
        return false;
    }

    void UpdateSpeed(float newSpeed)
    {
        if (navAgent != null && navAgent.gameObject != null)
        {
            navAgent.speed = newSpeed;
        }
    }


    public float GetCurrentSpeed()
    {
        return navAgent.velocity.magnitude;
    }
}
