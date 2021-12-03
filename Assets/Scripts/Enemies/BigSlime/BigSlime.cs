using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigSlime : Enemy
{
    public enum State
    {
        IDLE,
        CHASE,
        SHOOT,
        ATTACK,
        JUMPING,
        DEAD
    }

    private NavMeshAgent agent;
    private Rigidbody rb;
    private List<Heart> hearts = new List<Heart>();
    private int numOfHearts;

    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Transform ejectionPoint;

    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private float attackCooldown = 1.5f;
    private float attackCooldownTimer = 0f;
    [SerializeField] private float attackRange = 5f;

    private Vector3 jumpPosition;

    private float shootCooldown = 2f;
    private float shootCooldownTimer = 0f;

    [SerializeField] private int numOfJumpsBeforeShooting = 10;
    private int numOfJumps;

    private float jumpCooldown = 1f;
    private float jumpCooldownTimer = 0f;

    private State currentState;

    public State GetCurrentState()
    {
        return currentState;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        hearts.AddRange(GetComponentsInChildren<Heart>()); // FIGURE OUT HOW TO SUBSCRIBE TO EACH ONE OF THEM
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
        Debug.Log(numOfHearts);
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
                    if (numOfJumps > numOfJumpsBeforeShooting - 1)
                    {
                        SetState(State.SHOOT);
                        return;
                    }

                    if (Vector3.Distance(transform.position, player.position) < attackRange)
                    {
                        SetState(State.ATTACK);
                        return;
                    }

                    agent.SetDestination(player.position);

                    jumpCooldownTimer += Time.deltaTime;
                    if (jumpCooldownTimer > jumpCooldown)
                    {
                        Vector3 dirToPlayer = GetDirectionToPlayer();
                        Jump(dirToPlayer, 250, 100);
                        jumpCooldownTimer = 0f;
                    }

                    break;
                }
            case State.JUMPING:
                break;
            case State.SHOOT:
                {
                    shootCooldownTimer += Time.deltaTime;
                    if (shootCooldownTimer > shootCooldown)
                    {
                        GameObject slime = Instantiate(slimePrefab, ejectionPoint.position + transform.forward * 5f, Quaternion.identity);
                        Rigidbody slimeRigidBody = slime.GetComponent<Rigidbody>();
                        slimeRigidBody.AddForce(transform.forward * 100f);
                        SetState(State.CHASE);
                        shootCooldownTimer = 0f;
                    }
                    break;
                }
            case State.ATTACK:
                {
                    if (Vector3.Distance(transform.position, player.position) > attackRange)
                    {
                        SetState(State.CHASE);
                        return;
                    }

                    attackCooldownTimer += Time.deltaTime;
                    if (attackCooldownTimer > attackCooldown)
                    {
                        Attack();
                        attackCooldownTimer = 0f;
                    }

                    break;
                }
            case State.DEAD:
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
        if (collision.collider.CompareTag("Player"))
        {
            rb.AddForce(transform.forward * -400f);
        }

        /*        if (!jumped)
                    return;*/

        /*        if (state == State.JUMPING)
                {
                    SetState(State.IDLE);
                }*/

        if (currentState == State.JUMPING)
        {
            if (collision.collider.CompareTag("Floor"))
            {
                SetState(State.CHASE);
            }
        }
    }

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
            case State.SHOOT:
                DisableAgent();
                numOfJumps = 0;
                break;
            case State.ATTACK:
                attackCooldownTimer = attackCooldown; // ensure that slime immediately attacks player
                break;
            case State.DEAD:
                Destroy(gameObject);
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

    void Attack()
    {
        DisableAgent();

        rb.AddForce(transform.forward * 500f);
    }

    void Jump(Vector3 direction, float upMagnitude, float forwardMagnitude)
    {
        agent.SetDestination(transform.position); // Stops agent from moving

        DisableAgent();

        rb.AddForce(transform.up * upMagnitude + direction * forwardMagnitude);

        SetState(State.JUMPING);

        ++numOfJumps;
    }

    void OnDamagedEvent()
    {

    }

    private void OnHeartDestroyedEvent()
    {
        --numOfHearts;

        if (numOfHearts < 1)
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
