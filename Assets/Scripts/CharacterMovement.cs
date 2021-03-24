using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    // variable to store character animator component
    Animator animator;

    // variables to store optimized setter/getter paramter IDs
    int isWalkingHash;
    int isRunningHash;

    // variable to store the instance of the PlayerInput
    PlayerInput input;

    // variables to store input values
    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;
    bool jumpPressed;

    //Pointer to main camera
    Camera mainCam;

    //Smooth turning variables
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        input = new PlayerInput();

        input.CharacterControls.Movement.started += ctx =>
        {
            Debug.Log(ctx.ReadValue<Vector2>());
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y > 0;
        };
        input.CharacterControls.Movement.performed += ctx =>
        {
            Debug.Log(ctx.ReadValue<Vector2>());
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y > 0;
        };
        input.CharacterControls.Movement.canceled += ctx =>
        {
            Debug.Log(ctx.ReadValue<Vector2>());
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y > 0;
        };
        input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Jump.performed += ctx => jumpPressed = ctx.ReadValueAsButton();
    }


    // Start is called before the first frame update
    void Start()
    {
        // variable to store cahracter animator component
        animator = GetComponent<Animator>();

        // variables to store optimized setter/getter paramters IDs
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");


        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleRotation();
    }

    void handleRotation()
    {

        if((currentMovement.x != 0) && (currentMovement.y != 0))
        {
            float targetAngle = Mathf.Atan2(currentMovement.x, currentMovement.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        /* Old code that doesn't support camera movment
        // Current position of our character
        Vector3 currentPosition = transform.position;

        // the change in position our chracter should point to
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);

        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
        */
    }


    void handleMovement()
    {
        // get paramter values from animator
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        if ((!movementPressed || !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void OnEnable()
    {
        // enable the chracter controls action map
        input.CharacterControls.Enable();
    }

    void OnDisable()
    {
        // disable the chracter controls action map
        input.CharacterControls.Disable();
    }
}
