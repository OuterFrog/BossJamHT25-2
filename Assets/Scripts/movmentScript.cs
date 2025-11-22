using UnityEngine;
using UnityEngine.AI;

public class movmentScript : MonoBehaviour
{




    public NavMeshAgent agent;
    public Transform playArea;
    public bool UseStartPosAsPatrolArea;
    public Vector3 patrolArea;
    public float patrolRange;

    public float closeEnoughTolerance;

    Vector3 moveTo;

    public Transform enemy;

    [SerializeField] Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        moveTo = transform.position;

        if (UseStartPosAsPatrolArea)
        {
            patrolArea = moveTo;
        }
        
        anim.Play("WalkBlend");

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("walkAmount", agent.velocity.magnitude / agent.speed);

        if ((enemy.transform.position.x - moveTo.x) < closeEnoughTolerance && (enemy.transform.position.x - moveTo.x) > -closeEnoughTolerance && (enemy.transform.position.z - moveTo.z) < closeEnoughTolerance && (enemy.transform.position.z - moveTo.z) > -closeEnoughTolerance)
        {

            

            RandomPoint(patrolArea, patrolRange, out moveTo);

            agent.SetDestination(moveTo);

        }
    }


    public void StopMoving()
    {
        agent.isStopped = true;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 3000; i++)
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


    public void kill()
    {
        StopMoving();
        GameManager.singleton.EnemyIsKilled();
        anim.SetTrigger("dead");
        GetComponent<BoxCollider>().enabled = false;
        StopMoving();

        Invoke(nameof(DestroyEnemy), 2);

        transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>().Play();
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

}
