using UnityEngine;

public class GabbeTest : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Force), 5);
    }

    void Force()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * 1000);
    }
}
