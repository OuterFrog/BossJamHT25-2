using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject playerObject;

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
    }

    public void KillingMode()
    {
        
    }
}
