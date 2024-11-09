using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour 
{
    private Vector3 _nextLoc;

    public float range;

    private NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        RandomPoint(transform.position, range, out _nextLoc);
        _agent.SetDestination(_nextLoc);
    }

    // Update is called once per frame
    void Update()
    {
        // if the NPC's position is very close to the target position, 
        // set another position
        if (_agent.remainingDistance < 0.1f)
        {
            RandomPoint(transform.position, range, out _nextLoc);
            _agent.SetDestination(_nextLoc);
        }
    }
    /** 
     * Copied from the Unity Documentation
     */
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
