using UnityEngine;
using UnityEngine.InputSystem;

public class FPPlayer : MonoBehaviour
{
    public float walkSpeed;
    public float mouseSensitivity;
    public Vector2 walkVector;
    public Vector2 mouseVector;
    Vector2 DashVector;
    public Rigidbody rig;
    public Camera cam;
    float leftClickValue;

    public float attackPower;
    public float attackChargeTimeMax;
    float currentAttackChargeTime;
    public float cameraChargeMove;
    public float cameraReleaseMove;
    public bool inDash;
    public bool charging;

    float xRotation = 0;

    public InputActionReference moveInput;
    public InputActionReference mouseInput;
    public InputActionReference leftClick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        currentAttackChargeTime = 0;
        inDash = false;
        charging = false;
    }

    // Update is called once per frame
    void Update()
    {
        walkVector = moveInput.action.ReadValue<Vector2>();
        mouseVector = mouseInput.action.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
        leftClickValue = leftClick.action.ReadValue<float>();

        xRotation -= mouseVector.y;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseVector.x);
    }

    private void FixedUpdate()
    {
        Vector3 moveVector = new Vector3(0, rig.linearVelocity.y, 0);
        moveVector += walkVector.x * transform.right;
        moveVector += walkVector.y * transform.forward;
        moveVector = moveVector.normalized * walkSpeed * Time.fixedDeltaTime;

        rig.linearVelocity = moveVector;
    }

    void Charge()
    {
        if (currentAttackChargeTime < 0) currentAttackChargeTime = 0;

        if(leftClickValue > 0.01f && !inDash)
        {
            charging = true;
            currentAttackChargeTime += Time.deltaTime;
            cam.transform.position -= cam.transform.forward.normalized * cameraChargeMove * Time.deltaTime;

            if(currentAttackChargeTime >= attackChargeTimeMax)
            {
                inDash = true;
            }
        }
        else if(currentAttackChargeTime > 0)
        {
            charging = false;
            currentAttackChargeTime -= Time.deltaTime;
        }
    }
}
