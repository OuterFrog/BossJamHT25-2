using UnityEngine;

public class EnemyVisionScript : MonoBehaviour
{


    
    public float viewAnagleFOV;
    public float viewRange;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.singleton == null)
        {
            Debug.Log("no gamemanager ref found");
        }
        if (GameManager.singleton.GetPlayerObj() != null) {
            lookFor(GameManager.singleton.GetPlayerObj());
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
            if (Vector3.Dot(direction, transform.forward) < viewAnagleFOV / 180 || Vector3.Dot(direction, transform.forward) > -1 * viewAnagleFOV / 180) {

                Debug.Log("i can see you");

            }
            
        }
    }
}
