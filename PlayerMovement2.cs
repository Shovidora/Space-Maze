using UnityEngine;

public class PlayerMovement2 : MyMonoBehaviour
{
    private CharacterController controller;
    private PlayerAbilities playerAbilities;

    [Header("Movement:")]
    public float moveSpeedGeneral = 4f;
    public float moveSpeedForward = 4f;
    public float moveSpeedBackward = 2f;
    public float moveSpeedSides = 2f;
    public float moveSpeedPresentsWithShield = 0.9f;
    public float moveSpeedPresentsWithShieldOn = 0.7f;
    private float moveSpeed;
    public float detectionDistance = 1;
    public float detectionRadius = 1;
    public LayerMask obstacleMask;
    public Vector3 originCheckForObstacle;
    private Vector3 originCheckObstacle;
    private Vector3 moveDirection;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public bool isGrounded = false;
    public float jumpSpeed = 2f;
    public float radiusGroundCheck;
    public LayerMask groundLayers;
    public Transform groundCheck;
    public float radiusTopCheck;
    public LayerMask topLayers;
    public Transform topCheck;
    public float topHitYVelocityDownSensetivity;
    public Vector3 yVelocity = new();

    [Header("Rotation")]
    public float rotationSensitivity = 5f;
    public Vector2 yRotationLimits;
    public float yRotationSensitivity = 3f;
    //
    public Transform head;
    public Transform cameraTarget;
    public float originalY;
    public float cameraTargetYSensitivity;
    //
    public float xAxisRotation;
    public float xAxisRotationPercentage;

    [Header("Gizmos:")]
    public bool drawTopShepreCheck;
    public bool drawMovementObstacleCheck;

    // [Header("Add Ones:")]



    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAbilities = GetComponent<PlayerAbilities>();

        originalY = cameraTarget.localPosition.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Movement();
        Rotation();
        Gravity();
        
        MouseOnAndOff();
    }

    private void Movement()
    {
        // input:
        float moveInputZ = Input.GetAxis("Vertical");
        float moveInputX = Input.GetAxis("Horizontal");
        Vector3 inputMoveDir = Vector3.zero;

        if (moveInputZ > 0)
            inputMoveDir += moveInputZ * moveSpeedForward * transform.forward;
        else if (moveInputZ < 0)
            inputMoveDir += moveInputZ * moveSpeedBackward * transform.forward;

        if (moveInputX != 0)
            inputMoveDir += moveInputX * moveSpeedSides * transform.right;

        moveDirection = inputMoveDir;
        // ;

        // modifiers:
        moveSpeed = moveSpeedGeneral;
        if (playerAbilities.currentAbility == PlayerAbilities.AbilityState.Shield)
        {
            if (playerAbilities.shield.GetComponent<Shield>().inUse) moveSpeed *= moveSpeedPresentsWithShieldOn;
            else moveSpeed *= moveSpeedPresentsWithShield;
        }
        // ;   

        // obstacle:
        originCheckObstacle = transform.position + originCheckForObstacle;
        if (!IsObjectsAhead(originCheckObstacle, detectionRadius, moveDirection, detectionDistance, obstacleMask))
        {
            controller.Move(moveSpeed * Time.deltaTime * moveDirection);
            FindAnyObjectByType<OnPlayCanvas>().ShowObstacleAheadWarning(false);
        }
        else
        {
            FindAnyObjectByType<OnPlayCanvas>().ShowObstacleAheadWarning(true);
        }
        // ;
    }
    private void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * rotationSensitivity);

        // Rotate Head and Camera
        float mouseY = Input.GetAxis("Mouse Y") * yRotationSensitivity;
        xAxisRotation -= mouseY;
        xAxisRotation = Mathf.Clamp(xAxisRotation, -yRotationLimits.x, yRotationLimits.y);
        SetXAxisRotationPercentage();

        // head:
        head.localRotation = Quaternion.Euler(-xAxisRotation, 0f, 0f);

        // camera target:
        Vector3 pos = cameraTarget.localPosition;
        pos.y = originalY + xAxisRotation * cameraTargetYSensitivity;
        cameraTarget.localPosition = pos;
    }
    private void SetXAxisRotationPercentage()
    {
        if (xAxisRotation >= 0)
            xAxisRotationPercentage = xAxisRotation / yRotationLimits.y; // 0<X<1
        else
            xAxisRotationPercentage = /*-*/xAxisRotation / yRotationLimits.x; // negative value -1<X<0
    }
    private void Gravity()
    {
        if (Physics.CheckSphere(groundCheck.position, radiusGroundCheck, groundLayers))
            isGrounded = true;
        else
            isGrounded = false;

        if (isGrounded)
            yVelocity.y = 0;
        else
            yVelocity.y += gravity * Time.deltaTime;

        // Unity Error: // skip the last slow part in the pre-middle (top) of the jump
        if (!isGrounded && yVelocity.y < 1 && yVelocity.y > 0)
            yVelocity.y = gravity * Time.deltaTime;

        // jump:
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {     
            yVelocity.y += jumpSpeed;
            // Sound:
            AudioManager.Instance.PlayPlayerJump();
        }

        // top:
        if (!isGrounded && Physics.CheckSphere(topCheck.position, radiusTopCheck, topLayers))
        {
            if (yVelocity.y > 0 && yVelocity.y < 1)
                yVelocity.y = -1;
            else
                yVelocity.y = -yVelocity.y * topHitYVelocityDownSensetivity;
        }

        controller.Move(yVelocity * Time.deltaTime);
    }

    private void MouseOnAndOff()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    ///
    void OnDrawGizmos()
    {
        if (drawTopShepreCheck)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(topCheck.position, radiusTopCheck);
        }

        if (drawMovementObstacleCheck)
        {
            originCheckObstacle = transform.position + originCheckForObstacle;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(originCheckObstacle, detectionRadius);
            Vector3 end = originCheckObstacle + moveDirection * detectionDistance;
            Gizmos.DrawLine(originCheckObstacle, end);
            Gizmos.DrawWireSphere(end, detectionRadius);
        }
    }
}
