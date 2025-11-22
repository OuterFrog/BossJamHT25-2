using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField, Range(0,20)] float speed;
    [SerializeField] Vector3 offset;
    [SerializeField] Transform target;

    [SerializeField] bool lookAtTarget = false;

    void Start()
    {
        transform.position = GetTargetPos();
    }

    void Update()
    {
        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }

    void LateUpdate()
    {
        InterpolatePos();
    }

    Vector3 GetTargetPos()
    {
        return new Vector3(target.position.x, 0, target.position.z) + offset;
    }

    void InterpolatePos()
    {
        transform.position = Vector3.Lerp(transform.position, GetTargetPos(), speed * Time.deltaTime);
    }
}
