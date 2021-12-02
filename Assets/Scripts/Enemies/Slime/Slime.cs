using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : Enemy
{
    public enum State
    {
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

    private Vector3 jumpPosition;

    private State currentState;

    #region MonoBehaviour Callbacks

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        hearts.AddRange(GetComponentsInChildren<Heart>());
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

        SetState(State.IDLE);
    }

    protected override void Update()
    {
        switch (currentState)
        {
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

                    Vector3 dirToPlayer = GetDirectionToPlayer();
                    Jump(dirToPlayer, 250, 100);

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

    #endregion

    #region Public Methods

    public void SetState(State nextState)
    {
        currentState = nextState;

        switch (currentState)
        {
            case State.IDLE:
                DisableAgent();
                break;
            case State.CHASE:
                ActivateAgent();
                break;
            case State.JUMPING:
                break;
            case State.READY_TO_ATTACK:
                DisableAgent();
                break;
            case State.DEAD:

                break;
        }
    }

    #endregion

    #region Private Methods

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

    #endregion
}
