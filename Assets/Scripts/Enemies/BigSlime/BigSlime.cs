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
        ATTACK,
        RELAX,
        DEAD
    }

    private Rigidbody rb;
    private List<Heart> hearts = new List<Heart>();
    private int numOfHearts;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("State")]
    [SerializeField] private State currentState;

    [Header("Chase")]
    [SerializeField] private float moveCooldown = 2f;
    private float moveCooldownTimer = 0f;
    [SerializeField] private float moveForce = 10f;

    public bool isReadyToMove = false;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackForce = 100f;

    [Header("Relax")]
    [SerializeField] private float relaxTime = 4f;
    private float relaxTimer = 0f;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 50f;

    [Header("VFX")]
    [SerializeField] private ParticleSystem chargeVFX;
    [SerializeField] private ParticleSystem attackVFX;

    //private Material material;
    //private Color defaultColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hearts.AddRange(GetComponentsInChildren<Heart>()); // FIGURE OUT HOW TO SUBSCRIBE TO EACH ONE OF THEM
        //material = GetComponent<Renderer>().material;
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

        //defaultColor = material.GetColor("_BaseColor");

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
                    if (GetDistanceToPlayer() < attackRange && isReadyToMove)
                    {
                        SetState(State.ATTACK);
                        return;
                    }

                    FacePlayer();

                    moveCooldownTimer += Time.deltaTime;
                    if (moveCooldownTimer > moveCooldown)
                    {
                        animator.SetInteger("State", 1);
                        moveCooldownTimer = 0f;
                    }

                    break;
                }
            case State.ATTACK:
                {
                    if (GetDistanceToPlayer() > attackRange && isReadyToMove)
                    {
                        SetState(State.CHASE);
                        return;
                    }
                }
                break;
            case State.RELAX:
                {
                    relaxTimer += Time.deltaTime;
                    if (relaxTimer > relaxTime)
                    {
                        SetState(State.IDLE);
                        relaxTimer = 0f;
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

        if (rb.velocity.magnitude < 0.1f)
        {
            SetState(State.RELAX);
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
        if (currentState == State.ATTACK)
        {
            playerHealth.DealDamage();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnDamagedEvent()
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

    public void SetState(State nextState)
    {
        currentState = nextState;

        switch (currentState)
        {
            case State.IDLE:
                //material.SetColor("_BaseColor", defaultColor);
                break;
            case State.CHASE:
                //material.SetColor("_BaseColor", defaultColor);
                break;
            case State.ATTACK:
                //material.SetColor("_BaseColor", new Color(1, 0, 0, 0.5f));
                StartCoroutine("Attack");
                
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

    public void StartMove()
    {
        rb.AddForce((player.position - transform.position).normalized * moveForce, ForceMode.Impulse);
        attackVFX.Play();
    }

    public void SizeReverted()
    {
        attackVFX.Stop();
        animator.SetInteger("State", 0);
        isReadyToMove = true;
    }

    public void StartAttack()
    {
        rb.AddForce(transform.forward * attackForce, ForceMode.Impulse);
        attackVFX.Play();
        chargeVFX.Stop();
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        chargeVFX.Play();
        animator.SetInteger("State", 2);
    }
}
