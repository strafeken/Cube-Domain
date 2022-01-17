using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shifter : Enemy
{
    public enum State
    {
        SPAWNED,
        DISAPPEAR,
        EMERGE,
        ATTACKING,
        DEAD
    }

    private NavMeshAgent agent;
    private Rigidbody rb;
    private Heart heart;
    private BoxCollider boxCollider;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("State")]
    [SerializeField] private State currentState;

    [SerializeField] private float spawnBufferDuration = 2f;
    private float spawnedTimer = 0f;

    [SerializeField] private float rotationSpeed = 50f;

    [SerializeField] private float timeToDisappear = 1.5f;

    [SerializeField] private float timeToEmerge = 3f;

    [SerializeField] private float emergeForce = 10f;
    [SerializeField] private float emergeRange = 5f;
    [SerializeField] private float emergingSpeed = 10f;
    [SerializeField] private float minEmergeHeight = 1.5f;
    [SerializeField] private float maxEmergeHeight = 2f;
    [SerializeField] private float wallEmergeDistance = 35f;

    private LayerMask emergeMask;
    private Vector3 emergePosition;

    [Header("VFX")]
    [SerializeField] private GameObject disappearVFX;
    private GameObject disappearInstantiatedVFX;

    [SerializeField] private GameObject emergeVFX;
    private GameObject emergeInstantiatedVFX;

    [Header("Others")]
    private Vector3 centerOfArena;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        heart = GetComponentInChildren<Heart>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnEnable()
    {
        heart.OnHeartHit += OnDamagedEvent;
        heart.OnHeartDestroyed += OnHeartDestroyedEvent;
    }

    protected override void Start()
    {
        base.Start();

        centerOfArena = GameObject.FindGameObjectWithTag("CenterOfArena").GetComponent<Transform>().position;

        emergeMask = LayerMask.GetMask("EmergingPoints");
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case State.SPAWNED:
                {
                    // Move out of cage for (spawnBufferDuration)s
                    agent.SetDestination(centerOfArena);

                    spawnedTimer += Time.deltaTime;
                    if (spawnedTimer > spawnBufferDuration)
                    {
                        agent.enabled = false;
                        SetState(State.DISAPPEAR);
                    }
                }
                break;
            case State.DISAPPEAR:
                {
                    FacePlayer();
                }
                break;
            case State.EMERGE:
                {
                    FacePlayer();
                }
                break;
            case State.ATTACKING:
                {
                    if (transform.position.y < 0f)
                        SetState(State.DEAD);
                }
                break;
        }
    }

    void FixedUpdate()
    {

    }

    void OnDisable()
    {
        heart.OnHeartHit -= OnDamagedEvent;
        heart.OnHeartDestroyed -= OnHeartDestroyedEvent;
    }

    void OnTriggerEnter(Collider other)
    {
        switch (currentState)
        {
            case State.ATTACKING:
                {
                    // Hit the player
                    if (other.CompareTag("Player"))
                    {
                        playerHealth.DealDamage();
                    }
                    else if (other.CompareTag("Wall"))
                    {
                        //boxCollider.isTrigger = false;
                        //SetState(State.DISAPPEAR);
                    }
                    // Missed the player
                    else if (other.CompareTag("Floor"))
                    {
                        // Only solidify if it's already going down from attacking
                        if (rb.velocity.y < 0f)
                        {
                            boxCollider.isTrigger = false;
                            SetState(State.DISAPPEAR);
                        }
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Permeate after passing through player
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        switch (currentState)
        {
            case State.ATTACKING:
                {
                    if (other.CompareTag("Player"))
                    {
                        boxCollider.isTrigger = false;
                    }
                }
                break;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        switch (currentState)
        {
            /// When it already finished attacking
            case State.ATTACKING:
                {
                    // Hit the player
                    if (collision.collider.CompareTag("Player"))
                    {

                    }
                    // Missed the player
                    else if (collision.collider.CompareTag("Wall"))
                    {

                    }
                    // Missed the player
                    else if (collision.collider.CompareTag("Floor"))
                    {
                        SetState(State.DISAPPEAR);
                    }
                }
                break;
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
                break;
            case State.DISAPPEAR:
                StartCoroutine(DisappearCountdown());
                break;
            case State.EMERGE:
                StartCoroutine(Emerge());
                break;
            case State.ATTACKING:
                boxCollider.isTrigger = true;
                break;
            case State.DEAD:
                Destroy(gameObject);
                break;
        }
    }

    private void FacePlayer()
    {
        Vector3 dirToPlayer = GetDirectionToPlayer();
        if(!Equals(Vector3.zero, new Vector3(dirToPlayer.x, 0, dirToPlayer.z)))
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dirToPlayer.x, 0, dirToPlayer.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator DisappearCountdown()
    {
        rb.velocity = Vector3.zero;

        disappearInstantiatedVFX = Instantiate(disappearVFX, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.identity);

        yield return new WaitForSeconds(timeToDisappear);

        boxCollider.enabled = false;
        rb.isKinematic = true;

        animator.SetInteger("State", 1); // Start animation
    }

    public void Disappear()
    {
        Destroy(disappearInstantiatedVFX);

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        SetState(State.EMERGE);
    }

    private IEnumerator Emerge()
    {
        animator.SetInteger("State", 0); // Reset animation

        yield return new WaitForSeconds(timeToEmerge);

        // Get emerge position
        Vector3 randomPosition = GetRandomPosition();

        Collider[] hitColliders = Physics.OverlapSphere(randomPosition, emergeRange, emergeMask);

        int closestIndex = 0;

        if (hitColliders.Length > 0)
        {
            closestIndex = GetClosestCollider(hitColliders, randomPosition);

            emergePosition = hitColliders[closestIndex].ClosestPoint(randomPosition);

            Vector3 origin = new Vector3(0, emergePosition.y, 0);
            if(Vector3.Distance(emergePosition, origin) > wallEmergeDistance)
            {
                emergePosition.y = Mathf.Clamp(emergePosition.y, minEmergeHeight, maxEmergeHeight);
            }

            transform.position = emergePosition;
        }
        else
        {
            transform.position = new Vector3(player.position.x, 0f, player.position.z);
        }

        foreach (Transform child in transform)
            child.gameObject.SetActive(true);

        // Set emerge position
        if(hitColliders.Length > 0)
        {
            if(hitColliders[closestIndex].CompareTag("Wall"))
            {
                Vector3 originPoint = new Vector3(0, emergePosition.y, 0);

                emergeInstantiatedVFX = Instantiate(emergeVFX, emergePosition, Quaternion.identity);

                if (Physics.Raycast(originPoint, emergePosition, out RaycastHit hit, emergeMask))
                {
                    emergeInstantiatedVFX.transform.up = hit.normal;
                }

                StartCoroutine(Pass());
            }
            else
            {
                emergeInstantiatedVFX = Instantiate(emergeVFX, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.identity);
                transform.position -= Vector3.up; // Move down
                StartCoroutine(Rise());
            }
        }
        else
        {
            emergeInstantiatedVFX = Instantiate(emergeVFX, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.identity);
            transform.position -= Vector3.up; // Move down
            StartCoroutine(Rise());
        }
    }

    private int GetClosestCollider(Collider[] hitColliders, Vector3 randomPosition)
    {
        int closestIndex = -1;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            float newClosestDistance = Vector3.Distance(randomPosition, hitColliders[i].transform.position);

            if (newClosestDistance < closestDistance)
            {
                closestDistance = newClosestDistance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private IEnumerator Rise()
    {
        bool isFullyVisible = false;
        while (!isFullyVisible)
        {
            transform.position += Vector3.up * emergingSpeed * Time.deltaTime;

            if (Physics.CheckSphere(groundCheck.position, 0.001f, emergeMask))
                isFullyVisible = true;

            yield return null;
        }

        boxCollider.enabled = true;
        rb.isKinematic = false;

        Vector3 attackDir = (player.transform.position - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        Vector3 emergeDir = attackDir + Vector3.up * 0.1f;
        rb.AddForce(emergeDir * emergeForce, ForceMode.Impulse);

        Destroy(emergeInstantiatedVFX);

        SetState(State.ATTACKING);
    }

    private IEnumerator Pass()
    {
        bool isFullyVisible = false;
        while (!isFullyVisible)
        {
            transform.position += transform.forward * emergingSpeed * Time.deltaTime;

            if (Physics.CheckSphere(wallCheck.position, 0.001f, emergeMask))
                isFullyVisible = true;

            yield return null;
        }

        boxCollider.enabled = true;
        rb.isKinematic = false;

        Vector3 attackDir = GetDirectionToPlayer();
        attackDir.y = 0.1f;
        rb.AddForce(attackDir * emergeForce, ForceMode.Impulse);

        Destroy(emergeInstantiatedVFX);

        SetState(State.ATTACKING);
    }

    private Vector3 GetRandomPosition()
    {
        float x = player.position.x + Random.Range(-emergeRange, emergeRange);
        float y = Random.Range(0, maxEmergeHeight);
        float z = player.position.z + Random.Range(-emergeRange, emergeRange);

        return new Vector3(x, y, z);
    }
}
