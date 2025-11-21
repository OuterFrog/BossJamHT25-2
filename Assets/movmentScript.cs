using UnityEngine;
using UnityEngine.AI;

public class movmentScript : MonoBehaviour
{




    public NavMeshAgent agent;
    public Transform playArea;

    public float setMaxXAxis;
    public float setMaxZAxis;

    float maxXAxis;
    float maxZAxis;
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

        gotToX = enemy.transform.position.x;
        gotToZ = enemy.transform.position.z;

        enemy.transform.position = new Vector3(gotToX, enemy.position.z, gotToZ);


    }

    // Update is called once per frame
    void Update()
    {
        if ((enemy.transform.position.x - gotToX) < 1 && (enemy.transform.position.x - gotToX) > -1 && (enemy.transform.position.z - gotToZ) < 1 && (enemy.transform.position.z - gotToZ) > -1)
        {

            gotToX = Random.Range(-maxXAxis, maxXAxis);
            gotToZ = Random.Range(-maxZAxis, maxZAxis);
            Vector3 moveTo = new Vector3(gotToX, enemy.position.y, gotToZ);


            agent.SetDestination(moveTo);

        }
    }
}
