using UnityEngine;

public class EnemyVisionScript : MonoBehaviour
{


    public GameManager GameManager;
    public float viewAnagle;
    public float viewRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GetPlayerObj() != null) {
            lookFor(GameManager.GetPlayerObj());
        }
        else Debug.Log("no playerref");
    }


    void lookFor(GameObject player)
    {
        RaycastHit hit;

        Vector3 direction = player.transform.forward - transform.forward;

        Debug.DrawRay(transform.position, direction, Color.red);

        if(Physics.Raycast(transform.position, direction, out hit, viewRange))
        {
            if (Vector3.Dot(direction, transform.forward) < viewRange || Vector3.Dot(direction, transform.forward) > -1 * viewRange) {

                Debug.Log("i can see you");

            }
            
        }
    }
}
