using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : Enemy
{
    public enum State
    {
        SPAWNED,
        IDLE,
        CHASE,
        JUMPING,
        READY_TO_ATTACK,
        ATTACKING,
        DEAD
    }

    private NavMeshAgent agent;
    private Rigidbody rb;
    private Heart heart;



    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("State")]
    [SerializeField] private State currentState;

    [SerializeField] private float spawnBufferDuration = 2f;
    private float spawnedTimer = 0f;

    [SerializeField] private float attackCooldown = 1.5f;
    private float attackCooldownTimer = 0f;
    [SerializeField] private float attackRange = 5f;

    [SerializeField] private float jumpCooldown = 0.5f;
    private float jumpCooldownTimer = 0f;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private Vector2 normalJumpForce;
    [SerializeField] private Vector2 attackJumpForce;

    [Header("VFX")]
    [SerializeField] private GameObject attackVFX;
    private GameObject instantiatedVFX;

    private Vector3 jumpPosition;

    //private Material material;
    private Color defaultColor;

    private Vector3 resultantJumpForce;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        heart = GetComponentInChildren<Heart>();
        //material = GetComponent<Renderer>().material;
    }

    void OnEnable()
    {
        heart.OnHeartHit += OnDamagedEvent;
        heart.OnHeartDestroyed += OnHeartDestroyedEvent;
    }

    protected override void Start()
    {
        base.Start();

        //defaultColor = material.GetColor("_FresnelColor");

        SetState(State.SPAWNED);
        //SetState(State.IDLE);
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case State.SPAWNED:
                {
                    agent.SetDestination(player.position);

                    spawnedTimer += Time.deltaTime;
                    if (spawnedTimer > spawnBufferDuration)
                    {
                        agent.enabled = false;
                        SetState(State.IDLE);
                    }
                }
                break;
            case State.IDLE:
                FacePlayer();
                break;
            case State.CHASE:
                {
                    if (Vector3.Distance(transform.position, player.position) < attackRange)
                    {
                        SetState(State.READY_TO_ATTACK);
                        return;
                    }

                    jumpCooldownTimer += Time.deltaTime;
                    if (jumpCooldownTimer > jumpCooldown)
                    {
                        Vector3 dirToPlayer = GetDirectionToPlayer();
                        Jump(dirToPlayer, normalJumpForce.x, normalJumpForce.y);
                        jumpCooldownTimer = 0f;
                    }

                    break;
                }
            case State.JUMPING:
                break;
            case State.READY_TO_ATTACK:
                {
                    if (Vector3.Distance(transform.position, player.position) > attackRange)
                    {
                        SetState(State.CHASE);
                        return;
                    }

                    FacePlayer();

                    attackCooldownTimer += Time.deltaTime;
                    if (attackCooldownTimer > attackCooldown)
                    {
                        jumpPosition = transform.position;

                        Vector3 dirToPlayer = GetDirectionToPlayer();
                        Jump(dirToPlayer, attackJumpForce.x, attackJumpForce.y);

                        attackCooldownTimer = 0f;
                    }

                    break;
                }
            case State.ATTACKING:
                break;
            case State.DEAD:
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    void OnDisable()
    {
        heart.OnHeartHit -= OnDamagedEvent;
        heart.OnHeartDestroyed -= OnHeartDestroyedEvent;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (currentState == State.ATTACKING)
        {
            // Hit the player
            if (collision.collider.CompareTag("Player"))
            {
                // Bounce slime back
                Vector3 dirToInitialPos = (jumpPosition - player.position).normalized;
                rb.AddForce(dirToInitialPos * 100);
                playerHealth.DealDamage();
            }
            // Missed the player
            else if (collision.collider.CompareTag("Floor"))
            {
                SetState(State.IDLE);
            }
        }

        if (currentState == State.JUMPING)
        {
            if (collision.collider.CompareTag("Floor"))
            {
                SetState(State.IDLE);
            }
        }
    }

    private void OnDamagedEvent()
    {

    }

    private void OnHeartDestroyedEvent()
    {
        SetState(State.DEAD);
    }

    public void SetState(State nextState)
    {
        currentState = nextState;

        switch (currentState)
        {
            case State.SPAWNED:
                ResetAnimationToIdle();
                break;
            case State.IDLE:
                ResetAnimationToIdle();
                //material.SetColor("_FresnelColor", defaultColor);
                break;
            case State.CHASE:
                if(instantiatedVFX)
                    Destroy(instantiatedVFX);
                attackCooldownTimer = 0f;
                //material.SetColor("_FresnelColor", defaultColor);
                ResetAnimationToIdle();
                jumpCooldownTimer = 0f;
                break;
            case State.JUMPING:
                break;
            case State.READY_TO_ATTACK:
                //material.SetColor("_FresnelColor", new Color(1, 0, 0, 0.5f));
                ResetAnimationToIdle();
                instantiatedVFX = Instantiate(attackVFX, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.identity);
                break;
            case State.DEAD:
                ResetAnimationToIdle();
                break;
        }
    }

    private void FacePlayer()
    {
        Vector3 dirToPlayer = GetDirectionToPlayer();
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dirToPlayer.x, 0, dirToPlayer.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        // Change state once it is facing the player
        if (Quaternion.Angle(transform.rotation, lookRotation) < 0.01f)
        {
            switch (currentState)
            {
                case State.IDLE:
                    SetState(State.CHASE);
                    break;
            }
        }
    }

    private void Jump(Vector3 direction, float upMagnitude, float forwardMagnitude)
    {
        animator.SetInteger("State", 1);
        resultantJumpForce = transform.up * upMagnitude + direction * forwardMagnitude;
    }

    public void StartJump()
    {
        Destroy(instantiatedVFX);

        rb.AddForce(resultantJumpForce);

        if (currentState == State.READY_TO_ATTACK)
        {
            SetState(State.ATTACKING);
            return;
        }

        SetState(State.JUMPING);
    }

    private void ResetAnimationToIdle()
    {
        animator.SetInteger("State", 0);
    }
}
