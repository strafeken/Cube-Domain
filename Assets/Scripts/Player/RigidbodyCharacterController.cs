using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyCharacterController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Walk")]
    [SerializeField] private float movementSpeed = 12f;
    [SerializeField] private float movementMultiplier = 10f;

    [Header("Jump")]
    private bool jump;
    [SerializeField] private float jumpForce = 8f;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float fallMultiplier = 2.5f;

    private Vector2 walkInput;

    private Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        MovePlayer();
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection.normalized * movementSpeed * movementMultiplier, ForceMode.Acceleration);
        FallToGround();
    }

    private void MovePlayer()
    {
        moveDirection = transform.forward * walkInput.y + transform.right * walkInput.x;

        if (!isGrounded)
            return;

        if (jump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
    }

    private void FallToGround()
    {
        if (rb.velocity.y < 0f)
            rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
    }

    public void ReceiveInput(Vector2 _walkInput)
    {
        walkInput = _walkInput;
    }

    public void JumpOnGround(InputAction.CallbackContext context)
    {
        jump = true;
    }
}
