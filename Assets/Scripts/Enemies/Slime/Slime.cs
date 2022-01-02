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
    private List<Heart> hearts = new List<Heart>();
    private int numOfHearts;

    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private float attackCooldown = 1.5f;
    private float attackCooldownTimer = 0f;
    [SerializeField] private float attackRange = 5f;

    [SerializeField] private float jumpCooldown = 0.5f;
    private float jumpCooldownTimer = 0f;

    private Vector3 jumpPosition;

    private State currentState;

    private Material material;
    private Color defaultColor;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        hearts.AddRange(GetComponentsInChildren<Heart>());
        material = GetComponent<Renderer>().material;
    }

    void OnEnable()
    {
        for (int i = 0; i < hearts.Count; ++i)
        {
            hearts[i].OnHeartHit += OnDamagedEvent;
            hearts[i].OnHeartDestroyed += OnHeartDestroyedEvent;
        }
    }

    protected override void Start()
    {
        base.Start();

        numOfHearts = hearts.Count;

        defaultColor = material.GetColor("_FresnelColor");

        SetState(State.IDLE);
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case State.SPAWNED:

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
                    if(jumpCooldownTimer > jumpCooldown)
                    {
                        Vector3 dirToPlayer = GetDirectionToPlayer();
                        Jump(dirToPlayer, 250, 100);
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
                        Jump(dirToPlayer, 200, 200);

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
        for (int i = 0; i < hearts.Count; ++i)
        {
            hearts[i].OnHeartHit -= OnDamagedEvent;
            hearts[i].OnHeartDestroyed -= OnHeartDestroyedEvent;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(currentState == State.ATTACKING)
        {
            // Hit the player
            if (collision.collider.CompareTag("Player"))
            {
                // Bounce slime back
                Vector3 dirToInitialPos = (jumpPosition - player.position).normalized;
                rb.AddForce(dirToInitialPos * 200);
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

    public void SetState(State nextState)
    {
        currentState = nextState;

        switch (currentState)
        {
            case State.IDLE:
                material.SetColor("_FresnelColor", defaultColor);
                DisableAgent();
                break;
            case State.CHASE:
                material.SetColor("_FresnelColor", defaultColor);
                jumpCooldownTimer = 0f;
                ActivateAgent();
                break;
            case State.JUMPING:
                break;
            case State.READY_TO_ATTACK:
                material.SetColor("_FresnelColor", new Color(1, 0, 0, 0.5f));
                DisableAgent();
                break;
            case State.DEAD:

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
        agent.SetDestination(transform.position); // Stops agent from moving

        DisableAgent();

        rb.AddForce(transform.up * upMagnitude + direction * forwardMagnitude);

        if (currentState == State.READY_TO_ATTACK)
        {
            SetState(State.ATTACKING);
            return;
        }

        SetState(State.JUMPING);
    }

    private void OnDamagedEvent()
    {

    }

    private void OnHeartDestroyedEvent()
    {
        --numOfHearts;

        if(numOfHearts < 1)
        {
            SetState(State.DEAD);
        }
    }

    private void DisableAgent()
    {
        agent.SetDestination(transform.position);
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;

        rb.freezeRotation = true;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void ActivateAgent()
    {
        agent.nextPosition = rb.position;
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;

        rb.freezeRotation = false;
        rb.isKinematic = true;
        rb.useGravity = false;
    }
}
