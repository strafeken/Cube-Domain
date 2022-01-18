using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyCharacterController : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider bodyCollider;

    [Header("Walk")]
    [SerializeField] private float movementSpeed = 12f;
    [SerializeField] private float movementMultiplier = 10f;
    private Vector2 walkInput;
    private Vector3 moveDirection;

    [Header("Jump")]
    private bool jump;
    [SerializeField] private float jumpForce = 8f;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashDuration = 2f;
    private bool dashed = false;
    [SerializeField] private SwordAnimation sword;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bodyCollider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {

    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(groundCheck.position.y < 0f)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
        MovePlayer();
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection.normalized * movementSpeed * movementMultiplier, ForceMode.Acceleration);
        if(!dashed)
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
        if(dashed)
        {
            if (rb.velocity.y < 0f)
                rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier * 5f - 1f) * Time.deltaTime;
        }
        else
        {
            if (rb.velocity.y < 0f)
                rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
    }

    public void ReceiveInput(Vector2 _walkInput)
    {
        walkInput = _walkInput;
    }

    public void JumpOnGround(InputAction.CallbackContext context)
    {
        jump = true;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        sword.Dash();
    }

    public void StartDash()
    {
        StartCoroutine("DashAttack");
    }

    private IEnumerator DashAttack()
    {
        bodyCollider.isTrigger = true;
        rb.AddForce(Camera.main.transform.forward * dashForce, ForceMode.Impulse);
        dashed = true;
        yield return new WaitForSeconds(dashDuration);
        bodyCollider.isTrigger = false;
        rb.velocity = Vector3.zero;
    }
}
