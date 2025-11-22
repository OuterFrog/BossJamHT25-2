using UnityEngine;


public class PickUpScript : MonoBehaviour
{

    public Vector3[] SpawnPos;

    public void PickedUp()
    {
        Destroy(this.gameObject);
    }

    private void Start()
    {
        if (SpawnPos.Length > 0) { 
        
            int spanPosIndex = Random.Range(0, SpawnPos.Length);  
            
            transform.position = SpawnPos[spanPosIndex];
        }
    }
}
