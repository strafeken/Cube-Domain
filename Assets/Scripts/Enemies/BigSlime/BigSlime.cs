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
        RELAX,
        JUMPING,
        DEAD
    }

    private Rigidbody rb;
    private List<Heart> hearts = new List<Heart>();
    private int numOfHearts;

    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Transform ejectionPoint;

    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private float attackRange = 5f;

    private Vector3 jumpPosition;

    private float shootCooldown = 2f;
    private float shootCooldownTimer = 0f;

    [SerializeField] private int numOfJumpsBeforeShooting = 10;
    private int numOfJumps;

    private float jumpCooldown = 1f;
    private float jumpCooldownTimer = 0f;

    [SerializeField] private State currentState;

    private bool forceApplied;

    [SerializeField] private BigSlimeCollision collisionDetector;

    [SerializeField] private float idleTime = 4f;
    private float idleTimer = 0f;

    private bool isAttacking;

    private Material material;
    private Color defaultColor;

    public State GetCurrentState()
    {
        return currentState;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hearts.AddRange(GetComponentsInChildren<Heart>()); // FIGURE OUT HOW TO SUBSCRIBE TO EACH ONE OF THEM
        material = GetComponent<Renderer>().material;
    }

    void OnEnable()
    {
        collisionDetector.OnCollisionDetected += OnCollisionEvent;

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

        rb.freezeRotation = true;

        defaultColor = material.GetColor("_BaseColor");

        SetState(State.CHASE);
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                {
                    FacePlayer();
                }
                break;
            case State.CHASE:
                {
                    //if (numOfJumps > numOfJumpsBeforeShooting - 1)
                    //{
                    //    SetState(State.SHOOT);
                    //    return;
                    //}

                    if (Vector3.Distance(transform.position, player.position) < attackRange)
                    {
                        SetState(State.ATTACK);
                        return;
                    }

                    FacePlayer();

                    //rb.AddForce((player.position - transform.position).normalized * 100f, ForceMode.Acceleration);

                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, 1.5f, player.position.z), 2f * Time.deltaTime);

                    //agent.SetDestination(player.position);

                    //jumpCooldownTimer += Time.deltaTime;
                    //if (jumpCooldownTimer > jumpCooldown)
                    //{
                    //    Vector3 dirToPlayer = GetDirectionToPlayer();
                    //    Jump(dirToPlayer, 250, 100);
                    //    jumpCooldownTimer = 0f;
                    //}

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
                    if (Vector3.Distance(transform.position, player.position) > attackRange && !isAttacking)
                    {
                        SetState(State.CHASE);
                        return;
                    }
                }
                break;
            case State.RELAX:
                {
                    idleTimer += Time.deltaTime;
                    if (idleTimer > idleTime)
                    {
                        SetState(State.IDLE);
                        idleTimer = 0f;
                    }
                }
                break;
            case State.DEAD:
                break;
        }
    }
    void FixedUpdate()
    {
        if (currentState != State.ATTACK)
            return;

        if (!forceApplied)
            return;

        if (rb.velocity.magnitude < 0.1f)
        {
            SetState(State.RELAX);
        }
    }

    void OnDisable()
    {
        collisionDetector.OnCollisionDetected -= OnCollisionEvent;

        for (int i = 0; i < hearts.Count; ++i)
        {
            hearts[i].OnHeartHit -= OnDamagedEvent;
            hearts[i].OnHeartDestroyed -= OnHeartDestroyedEvent;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void SetState(State nextState)
    {
        currentState = nextState;

        switch (currentState)
        {
            case State.IDLE:
                material.SetColor("_BaseColor", defaultColor);
                rb.isKinematic = true;
                isAttacking = false;
                forceApplied = false;
                break;
            case State.CHASE:
                material.SetColor("_BaseColor", defaultColor);
                rb.isKinematic = true;
                break;
            case State.SHOOT:
                material.SetColor("_BaseColor", defaultColor);
                numOfJumps = 0;
                break;
            case State.ATTACK:
                material.SetColor("_BaseColor", new Color(1, 0, 0, 0.5f));
                rb.isKinematic = false;
                StartCoroutine("ApplyForce");
                break;
            case State.DEAD:
                Destroy(gameObject);
                break;
        }
    }

    private IEnumerator ApplyForce()
    {
        isAttacking = true;

        rb.AddForce(transform.forward * 3000f);

        yield return new WaitForSeconds(0.1f);

        forceApplied = true;

        yield return new WaitForSeconds(1f);

        if (rb.velocity.magnitude > 0f)
            rb.velocity = Vector3.zero;
    }

    private void FacePlayer()
    {
        Vector3 dirToPlayer = GetDirectionToPlayer();
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dirToPlayer.x, 0, dirToPlayer.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

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

    void Jump(Vector3 direction, float upMagnitude, float forwardMagnitude)
    {
        //agent.SetDestination(transform.position); // Stops agent from moving

        //DisableAgent();

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

    private void OnCollisionEvent()
    {
        if (currentState == State.ATTACK)
        {
            playerHealth.DealDamage();
        }
    }
}
