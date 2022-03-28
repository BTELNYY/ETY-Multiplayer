using UnityEngine;
using Mirror;

[RequireComponent(typeof(CharacterController))]

public class MovementScript : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float crouchSpeedDivider = 2f;
    public float crouchWhileSprintingDivider = 4f;
    [Header("Keybinds")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;
    [Header("Other Settings")]
    public bool canMove = true;
    public Camera playerCamera;

    Transform player;

    //private members
    PlayerScript playerScript;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    
    [Client]
    public override void OnStartLocalPlayer()
    {
        PlayerScript playerScript = GetComponent<PlayerScript>();
        player = playerScript.GetPLayerTransform();
        characterController = player.GetComponent<CharacterController>();
        playerCamera = playerScript.GetPlayerCamera();
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 1f, 0);
        playerCamera = Camera.main;
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    [Client]
    void Update()
    {
        if (!isLocalPlayer) { return; }
        
            
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);    
        Vector3 right = transform.TransformDirection(Vector3.right);
            // Press Left Shift to run
        bool isRunning = Input.GetKey(runKey);
        bool isCrouching = Input.GetKey(crouchKey);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
        if (isCrouching && !isRunning)
        {
         //if the player is crouching, make them go slower also play anims or something
            curSpeedX /= crouchSpeedDivider;
        }
        else if (isCrouching && isRunning)
        {
                //if the player is crouching and running, make them go slower also play anims or something
            curSpeedX /= crouchWhileSprintingDivider;
        }
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);





        if (Input.GetKey(jumpKey) && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
             moveDirection.y = movementDirectionY;
        }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        
    }
}