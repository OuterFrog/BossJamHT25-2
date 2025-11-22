using UnityEngine;
using UnityEngine.AI;

public class movmentScript : MonoBehaviour
{




    public NavMeshAgent agent;
    public Transform playArea;

    public float setMaxXAxis;
    public float setMaxZAxis;

    public float setMinXAxis;
    public float setMinZAxis;

    public float closeEnoughTolerance;

    float maxXAxis;
    float maxZAxis;
    float minXAxis;
    float minZAxis;
    float gotToX;
    float gotToZ;
    public Transform enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //maxXAxis = playArea.localScale.x / 2;
        //maxZAxis = playArea.localScale.z / 2;

        maxXAxis = setMaxXAxis;
        maxZAxis = setMaxZAxis;

        minXAxis = setMaxXAxis;
        minZAxis = setMaxZAxis;

        gotToX = enemy.transform.position.x;
        gotToZ = enemy.transform.position.z;

        enemy.transform.position = new Vector3(gotToX, enemy.position.z, gotToZ);


    }

    // Update is called once per frame
    void Update()
    {
        if ((enemy.transform.position.x - gotToX) < closeEnoughTolerance && (enemy.transform.position.x - gotToX) > -closeEnoughTolerance && (enemy.transform.position.z - gotToZ) < closeEnoughTolerance && (enemy.transform.position.z - gotToZ) > -closeEnoughTolerance)
        {

            gotToX = Random.Range(minXAxis, maxXAxis);
            gotToZ = Random.Range(minZAxis, maxZAxis);
            Vector3 moveTo = new Vector3(gotToX, enemy.position.y, gotToZ);


            agent.SetDestination(moveTo);

        }
    }
}
