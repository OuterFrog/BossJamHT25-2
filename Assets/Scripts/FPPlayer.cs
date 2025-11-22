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
    public Transform cameraHolder;

    float xRotation = 0;

    public InputActionReference moveInput;
    public InputActionReference mouseInput;
    public InputActionReference leftClick;

    public float maxChargeTime;
    public float maxDashTime;
    ClockTimer chargeTimer;
    ClockTimer dashTimer;

    float currentChargePower;
    public float maxChargePower;
    public bool isDashing;

    public float camDefaultFOV;
    public float camMAXFOV;
    float currentDefultFOV;

    public GameObject attackCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        currentWalkSpeed = walkSpeed;
        currentChargePower = 0;
        isDashing = false;

        chargeTimer = new ClockTimer(maxChargeTime);
        dashTimer = new ClockTimer(maxDashTime);

        attackCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        walkVector = moveInput.action.ReadValue<Vector2>();
        mouseVector = mouseInput.action.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
        leftClickValue = leftClick.action.ReadValue<float>();

        CameraMovement();
        Charge();
    }

    private void FixedUpdate()
    {
        if(!isDashing) Movement();
    }

    void Charge()
    {
        if (isDashing)
        {
            attackCollider.SetActive(true);
            dashTimer.Tick(Time.deltaTime);

            if (dashTimer.timeIsUp)
            {
                attackCollider.SetActive(false);

                currentWalkSpeed = walkSpeed;
                isDashing = false;
                chargeTimer.Reset();
                dashTimer.Reset();

                cam.fieldOfView = camDefaultFOV;
            }

            return;
        }

        if (leftClickValue > 0.01f)
        {
            chargeTimer.Tick(Time.deltaTime);

            chargeDirection = cam.transform.forward;
            chargeDirection.y = 0;
            chargeDirection.Normalize();

            currentWalkSpeed = 0.4f * walkSpeed;

            cam.fieldOfView = Mathf.Lerp(camDefaultFOV, camMAXFOV, 1 - chargeTimer.currentTime / chargeTimer.maxTime);
            currentChargePower = Mathf.Lerp(0, maxChargePower, 1 - chargeTimer.currentTime / chargeTimer.maxTime);
        }

        else if (chargeTimer.currentTime < chargeTimer.maxTime)
        {
            isDashing = true;
            FlyForward();
        }
    }

    void Movement()
    {
        Vector3 moveVector = new Vector3(0, 0, 0);
        moveVector += walkVector.x * cameraHolder.transform.right;
        moveVector += walkVector.y * cameraHolder.transform.forward;
        moveVector = moveVector.normalized * currentWalkSpeed * Time.fixedDeltaTime;

        rig.linearVelocity = moveVector;
    }

    void CameraMovement()
    {
        xRotation -= mouseVector.y;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        cameraHolder.transform.Rotate(Vector3.up * mouseVector.x);
    }

    void FlyForward()
    {
        rig.AddForce(chargeDirection * currentChargePower * 100);
        Debug.Log("ATTACKED!");
    }

    private void OnTriggerEnter(Collider other)
    {


        if(other.gameObject.layer == 7)
        {
            other.GetComponent<EnemyVisionScript>().kill();
        }
    }
}
