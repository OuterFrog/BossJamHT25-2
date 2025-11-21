using UnityEngine;
using UnityEngine.InputSystem;

public class FPPlayer : MonoBehaviour
{
    public float walkSpeed;
    public float currentWalkSpeed;
    public float mouseSensitivity;
    public Vector2 walkVector;
    public Vector2 mouseVector;
    Vector2 DashVector;
    public Rigidbody rig;
    public Camera cam;
    float leftClickValue;
    public Vector3 defaultCamPosition;
    public Vector3 camDashStartPosition;
    Vector3 chargeDirection;

    public float chargePower;
    public float attackChargeTimeMax;
    float currentAttackChargeTime;
    public float cameraChargeFlybackTime;
    public float currentCameraChargeFlybackTime;
    public float cameraChargeMove;
    public bool inDash;
    public bool charging;
    public bool flyForward;

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
        currentWalkSpeed = walkSpeed;
        currentCameraChargeFlybackTime = cameraChargeFlybackTime;
        flyForward = false;
    }

    // Update is called once per frame
    void Update()
    {
        walkVector = moveInput.action.ReadValue<Vector2>();
        mouseVector = mouseInput.action.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
        leftClickValue = leftClick.action.ReadValue<float>();

        if (!charging) CameraMovement();
        Charge();
    }

    private void FixedUpdate()
    {
        Movement();
        FlyForward();
    }

    void Charge()
    {
        if(leftClickValue > 0.01f && !inDash)
        {
            charging = true;
            currentAttackChargeTime += Time.deltaTime;
            cam.transform.position -= cam.transform.forward.normalized * cameraChargeMove * Time.deltaTime;
            camDashStartPosition = cam.transform.position;
            defaultCamPosition = transform.position + new Vector3(0, 1.5f, 0.5f);
            chargeDirection = cam.transform.forward;
            chargeDirection.y = 0;
            chargeDirection = chargeDirection.normalized;

            if (currentAttackChargeTime >= attackChargeTimeMax)
            {
                inDash = true;
            }

            cam.transform.rotation = Quaternion.Euler(new Vector3(0.01f, cam.transform.rotation.y, cam.transform.rotation.z));
        }
        else if(currentAttackChargeTime > 0)
        {
            charging = false;
            inDash = true;

            currentCameraChargeFlybackTime += Time.deltaTime;
            float cameraReturnCheck = currentCameraChargeFlybackTime / cameraChargeFlybackTime;
            cam.transform.position = Vector3.Lerp(camDashStartPosition, defaultCamPosition, cameraReturnCheck);

            if(cameraReturnCheck >= 1)
            {
                flyForward = true;
            }

        }
    }

    void Movement()
    {
        Vector3 moveVector = new Vector3(0, rig.linearVelocity.y, 0);
        moveVector += walkVector.x * transform.right;
        moveVector += walkVector.y * transform.forward;
        moveVector = moveVector.normalized * currentWalkSpeed * Time.fixedDeltaTime;

        rig.linearVelocity = moveVector;
    }

    void CameraMovement()
    {
        xRotation -= mouseVector.y;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseVector.x);
    }

    void FlyForward()
    {
        if(flyForward)
        {
            rig.AddForce(chargeDirection * chargePower, ForceMode.Impulse);

            flyForward = false;
            charging = false;
            inDash = false;
            currentCameraChargeFlybackTime = 0;
            currentAttackChargeTime = 0;
        }
    }
}
