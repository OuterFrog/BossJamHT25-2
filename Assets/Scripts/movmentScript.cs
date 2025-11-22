using UnityEngine;
using UnityEngine.AI;

public class movmentScript : MonoBehaviour
{




    public NavMeshAgent agent;
    public Transform playArea;

    public Vector3 patrolArea;
    public float patrolRange;

    public float closeEnoughTolerance;

    Vector3 moveTo;

    public Transform enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        moveTo = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if ((enemy.transform.position.x - moveTo.x) < closeEnoughTolerance && (enemy.transform.position.x - moveTo.x) > -closeEnoughTolerance && (enemy.transform.position.z - moveTo.z) < closeEnoughTolerance && (enemy.transform.position.z - moveTo.z) > -closeEnoughTolerance)
        {


            

            RandomPoint(patrolArea, patrolRange, out moveTo);

            agent.SetDestination(moveTo);

        }
    }




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
