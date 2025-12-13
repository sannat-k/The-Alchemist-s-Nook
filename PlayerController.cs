using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    public float rotationSpeed = 30.0f;
    public float speed = 6.0f;
    public float gravity = 20.0f;

    Rigidbody rb;
    Vector2 moveInput;
    InputSystem_Actions controls;

    [Header("Inventory")]
    public Inventory playerInventory;

    void Awake()
    {
        //Time.timeScale = 1;
        animator = GetComponent<Animator>();
        //characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        controls = new InputSystem_Actions();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    void OnEnable()
    {
        // Enable the "Player" action map
        controls.Player.Enable();

        // Subscribe to the "Move" action's performed and canceled events
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCancled;
    }

    void OnDisable()
    {
        // Unsubscribe from the "Move" action's events
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCancled;
        // Disable the "Player" action map
        controls.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        animator.SetFloat("ForwardSpeed", moveInput.y);
        animator.SetFloat("RightSpeed", moveInput.x);
    }

    private void OnMoveCancled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
        animator.SetFloat("ForwardSpeed", 0.0f);
        animator.SetFloat("RightSpeed", 0f);

    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Only rotate and move if there is input
        if (move.sqrMagnitude > 0.001f)
        {
            // Calculate the target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            // Smoothly rotate towards the target direction
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
            // Move in the input direction
            rb.MovePosition(rb.position + move.normalized * speed * Time.fixedDeltaTime);

        }
    }

}
