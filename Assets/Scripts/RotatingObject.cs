using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] Vector3 rotSpeed;

    void Update()
    {
        transform.Rotate(rotSpeed * Time.deltaTime);
    }
}
