using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerObject;

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
