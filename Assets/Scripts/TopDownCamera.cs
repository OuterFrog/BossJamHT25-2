using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField, Range(0,20)] float speed;
    [SerializeField] Vector3 offset;
    [SerializeField] Transform target;

    [SerializeField] bool lookAtTarget = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }

    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z) + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.fixedDeltaTime);
    }
}
