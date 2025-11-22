using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownPlayer : MonoBehaviour
{
    [SerializeField] InputActionReference moveInput;

    [SerializeField, Range(0, 500)] float speed;

    [SerializeField] float rotSpeed;

    Rigidbody rb;
    Transform visualTransform;

    Quaternion targetRot = Quaternion.Euler(0,0,0);

    [SerializeField] Animator anim;
    [SerializeField] Animation anim2;

    float animWalkSpeed = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        visualTransform = transform.GetChild(0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //anim2.Play("walkLoopAnim");
    }

    // Update is called once per frame
    void Update()
    {   
        Vector2 inputVector = moveInput.action.ReadValue<Vector2>();
        if(inputVector.magnitude > 0)
        {
            visualTransform.rotation = Quaternion.Lerp(visualTransform.rotation, targetRot, rotSpeed * Time.deltaTime);
        }

        anim.SetBool("isWalking", rb.linearVelocity.magnitude > 0);
    }

    void FixedUpdate()
    {
        Vector2 inputVector = moveInput.action.ReadValue<Vector2>();
        Vector3 dirVector = new Vector3(inputVector.x, 0, inputVector.y);
        rb.linearVelocity = speed * dirVector * Time.fixedDeltaTime;

        animWalkSpeed = rb.linearVelocity.magnitude / (speed * Time.fixedDeltaTime);

        if(rb.linearVelocity.magnitude > 0)
        {
            Vector2 dir = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).normalized;
            float rot = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            targetRot = Quaternion.Euler(0,rot,0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pickup")
        {
            Debug.Log("Picked up");
            other.transform.parent.gameObject.GetComponent<PickUpScript>().PickedUp();
            FindFirstObjectByType<GameManager>().KillingMode();
        }
    }
}
