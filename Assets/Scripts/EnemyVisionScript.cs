using UnityEngine;

public class EnemyVisionScript : MonoBehaviour
{


    GameManager gameManager = null;
    public float viewAnagle;
    public float viewRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager == null)
        {
            Debug.Log("no gamemanager ref found");
        }
        if (gameManager.GetPlayerObj() != null) {
            lookFor(gameManager.GetPlayerObj());
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
