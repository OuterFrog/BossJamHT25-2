using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownPlayer : MonoBehaviour
{
    [SerializeField] InputActionReference moveInput;

    [SerializeField, Range(0, 500)] float speed;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        Vector2 inputVector = moveInput.action.ReadValue<Vector2>();
        Vector3 dirVector = new Vector3(inputVector.x, 0, inputVector.y);
        rb.linearVelocity = speed * dirVector * Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pickup")
        {
            Debug.Log("Picked up shit");
        }
    }
}
