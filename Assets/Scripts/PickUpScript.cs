using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public void PickedUp()
    {
        Destroy(this.gameObject);
    }
}
