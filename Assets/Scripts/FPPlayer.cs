using UnityEngine;
using UnityEngine.InputSystem;

public class FPPlayer : MonoBehaviour
{
    public float walkSpeed;
    public Vector2 inputVector;
    public Rigidbody rig;
    public Camera cam;

    public InputActionReference inputAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputVector = inputAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 moveVector = new Vector3(0, rig.linearVelocity.y, 0);
        moveVector.x = inputVector.x * cam.transform.right.x * walkSpeed * Time.deltaTime;
        moveVector.z = inputVector.y * cam.transform.forward.z * walkSpeed * Time.deltaTime;

        rig.linearVelocity = moveVector;
    }
}
