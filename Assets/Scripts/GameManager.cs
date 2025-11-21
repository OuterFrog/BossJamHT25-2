using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject playerObject;
    GameObject topDownCamera;

    [SerializeField] GameObject fpPlayerPrefab;

    bool killingMode = false;

    public GameObject GetPlayerObj()
    {
        return playerObject;
    }

    void Awake()
    {
        if(playerObject == null)
        {
            playerObject = FindFirstObjectByType<TopDownPlayer>().gameObject;
        }

        if(topDownCamera == null)
        {
            topDownCamera = FindFirstObjectByType<TopDownCamera>().gameObject;
        }
    }

    public void KillingMode()
    {   
        if(killingMode) return;

        killingMode = true;

        GameObject oldPlayer = playerObject;
        playerObject = Instantiate(fpPlayerPrefab);
        fpPlayerPrefab.transform.position = oldPlayer.transform.position + new Vector3(0, 10, 0);
        
        Destroy(oldPlayer);
        Destroy(topDownCamera);
    }
}
