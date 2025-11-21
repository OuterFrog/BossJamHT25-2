using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

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
        singleton = this;

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
        fpPlayerPrefab.transform.position = oldPlayer.transform.position;
        
        Destroy(oldPlayer);
        Destroy(topDownCamera);
    }
}
