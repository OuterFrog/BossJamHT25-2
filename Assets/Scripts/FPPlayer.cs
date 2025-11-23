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
    public float zoomBackTime;
    ClockTimer chargeTimer;
    ClockTimer dashTimer;
    ClockTimer zoomBackTimer;
    bool isZoomingBack;

    float currentChargePower;
    public float maxChargePower;
    public bool isDashing;

    public float camDefaultFOV;
    public float camMAXFOV;
    float currentDefultFOV;

    public GameObject attackCollider;

    public Collider playerColider;

    bool isPlayingChargeUp;
    public AudioSource soundSource;
    public AudioClip[] chargeSounds;
    public AudioClip chargeUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        currentWalkSpeed = walkSpeed;
        currentChargePower = 0;
        isDashing = false;

        chargeTimer = new ClockTimer(maxChargeTime);
        dashTimer = new ClockTimer(maxDashTime);
        zoomBackTimer = new ClockTimer(zoomBackTime);

        attackCollider.SetActive(false);
        isZoomingBack = false;
        isPlayingChargeUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.singleton.dead) return;

        walkVector = moveInput.action.ReadValue<Vector2>();
        mouseVector = mouseInput.action.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
        leftClickValue = leftClick.action.ReadValue<float>();

        CameraMovement();
        Charge();
        Debug.Log(playerColider.excludeLayers.value);
        if(isZoomingBack)
        {
            zoomBackTimer.Tick(Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(camMAXFOV, camDefaultFOV, 1 - zoomBackTimer.currentTime / zoomBackTimer.maxTime);

            if (zoomBackTimer.timeIsUp)
            {
                isZoomingBack = false;
                isDashing = false;
                chargeTimer.Reset();
                dashTimer.Reset();
                zoomBackTimer.Reset();

                cam.fieldOfView = camDefaultFOV;
            }
        }
    }

    private void FixedUpdate()
    {
        if(GameManager.singleton.dead) return;

        if(!isDashing) Movement();
    }

    void Charge()
    {
        if (isDashing)
        {
            attackCollider.SetActive(true);
            dashTimer.Tick(Time.deltaTime);
            playerColider.excludeLayers = LayerMask.GetMask("Enemy");


            if (dashTimer.timeIsUp)
            {
                attackCollider.SetActive(false);
                playerColider.excludeLayers = LayerMask.GetMask("Nothing");
                currentWalkSpeed = walkSpeed;
                isZoomingBack = true;
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
        SFXManager.singleton.PlayOneOf(1,2,3);
        rig.AddForce(chargeDirection * currentChargePower * 100);
        Debug.Log("ATTACKED!");
    }

    private void OnTriggerEnter(Collider other)
    {


        if(other.gameObject.layer == 7)
        {
            SFXManager.singleton.PlayOneOf(5,6,7,8,9,10,11);
            other.GetComponent<movmentScript>().kill();
        }
    }

    public void Die()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
