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

    private LayerMask emergeMask;
    private Vector3 emergePosition;

    [Header("VFX")]
    [SerializeField] private GameObject disappearVFX;
    private GameObject disappearInstantiatedVFX;

    [SerializeField] private GameObject emergeVFX;
    private GameObject emergeInstantiatedVFX;

    [Header("Others")]
    private Vector3 centerOfArena;

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

                }
                break;
            case State.EMERGE:
                {

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
    }

    public void Disappear()
    {
        boxCollider.enabled = false;
        rb.isKinematic = true;

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        SetState(State.EMERGE);
    }

    private IEnumerator DisappearCountdown()
    {
        yield return new WaitForSeconds(timeToDisappear);

        animator.SetInteger("State", 1); // Start animation
    }

    private IEnumerator Emerge()
    {
        animator.SetInteger("State", 0);

        yield return new WaitForSeconds(timeToEmerge);

        Vector3 randomPosition = GetRandomPosition();

        Collider[] hitColliders = Physics.OverlapSphere(randomPosition, 5f, emergeMask);

        if(hitColliders.Length > 0)
        {
            emergePosition = hitColliders[0].ClosestPoint(randomPosition);
            transform.position = emergePosition;
        }
        else
        {
            transform.position = new Vector3(player.position.x, 0f, player.position.z);
        }

        foreach (Transform child in transform)
            child.gameObject.SetActive(true);

        if(hitColliders[0].CompareTag("Wall"))
        {
            boxCollider.enabled = false;
            rb.isKinematic = true;

            Vector3 dirToEmerge = Vector3.zero - emergePosition; 
            dirToEmerge.y = 0f;

            rb.AddForce(dirToEmerge.normalized * emergeForce, ForceMode.Impulse);
        }
        else
        {
            boxCollider.enabled = true;
            rb.isKinematic = false;

            Vector3 dirToEmerge = player.position - emergePosition;

            rb.AddForce(dirToEmerge * emergeForce, ForceMode.Impulse);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = player.position.x + Random.Range(-emergeRange, emergeRange);
        float y = Random.Range(0, emergeRange);
        float z = player.position.z + Random.Range(-emergeRange, emergeRange);

        return new Vector3(x, y, z);
    }
}
