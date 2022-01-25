using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Twirler : Enemy
{
    public enum State
    {
        IDLE,
        CHASE,
        FIREBALL,
        MULTIPLE_FIREBALL,
        SPIN,
        SWIRL,
        READY_TO_ATTACK,
        DEAD
    }

    private FollowCurveRoute northCube;
    private FollowCurveRoute southCube;
    private FollowCurveRoute eastCube;
    private FollowCurveRoute westCube;

    private ControlBezierCurve nRoute;
    private ControlBezierCurve sRoute;
    private ControlBezierCurve eRoute;
    private ControlBezierCurve wRoute;

    private Heart heart;

    [Header("VFX")]
    [SerializeField] private GameObject swirlVFX;

    [Header("General")]
    [SerializeField] private State currentState;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float movementSpeed = 5f;

    [Header("Fireball")]
    [SerializeField] private float shootCooldown = 1f;
    private float shootCooldownTimer = 0f;
    private bool shot;
    private bool southHitPlayer;

    [Header("Spin")]
    [SerializeField] private float spinAttackRotationSpeed = 100f;
    [SerializeField] private float spinAttackMovementSpeed = 10f;
    [SerializeField] private float spinAttackRange = 10f;
    private Vector3 spinAttackEndPoint;

    public Action OnChangeToIdleState;
    public Action OnChangeToChaseState;
    public Action OnChangeToFireballState;
    public Action OnChangeToMultipleFireballState;
    public Action OnChangeToSpinState;

    void Awake()
    {
        //northCube = transform.Find("NorthCube").GetComponent<FollowCurveRoute>();
        southCube = transform.Find("SouthCube").GetComponent<FollowCurveRoute>();
        eastCube = transform.Find("EastCube").GetComponent<FollowCurveRoute>();
        westCube = transform.Find("WestCube").GetComponent<FollowCurveRoute>();

        Transform routes = transform.Find("TwirlerCurveAttackRoute");
        sRoute = routes.Find("SouthRoute").GetComponent<ControlBezierCurve>();
        eRoute = routes.Find("EastRoute").GetComponent<ControlBezierCurve>();
        wRoute = routes.Find("WestRoute").GetComponent<ControlBezierCurve>();
    
        heart = GetComponentInChildren<Heart>();
    }

    protected override void Start()
    {
        base.Start();

        heart.OnHeartHit += OnDamagedEvent;
        heart.OnHeartDestroyed += OnHeartDestroyedEvent;

        southCube.OnEndPointReached += OnProjectileReachEnd;
        eastCube.OnEndPointReached += OnProjectileReachEnd;
        westCube.OnEndPointReached += OnProjectileReachEnd;

        SetState(State.IDLE);
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                {
                    Quaternion lookRotation = GetLookRotation();
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                    // Once it is facing the player
                    if (Quaternion.Angle(transform.rotation, lookRotation) < 0.01f)
                    {
                        if (GetDistanceToPlayer() < spinAttackRange)
                        {
                            SetState(State.SPIN);
                            break;
                        }

                        // When it hasn't shot a projectile or has hit a projectile
                        if (!shot)
                        {
                            SetState(State.FIREBALL);
                            break;
                        }
                        else if (shot && southHitPlayer)
                        {
                            shootCooldownTimer += Time.deltaTime;
                            if (shootCooldownTimer > shootCooldown)
                            {
                                SetState(State.FIREBALL);
                                shootCooldownTimer = 0f;
                                break;
                            }
                        }
                        // When the shot misses
                        else if (!southHitPlayer)
                        {
                            SetState(State.MULTIPLE_FIREBALL);
                            break;
                        }
                    }
                }
                break;
            case State.CHASE:
                {
                    Quaternion lookRotation = GetLookRotation();
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                    transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);

                    if (GetDistanceToPlayer() < spinAttackRange)
                    {
                        SetState(State.SPIN);
                        return;
                    }
                }
                break;
            case State.FIREBALL:
                break;
            case State.MULTIPLE_FIREBALL:
                break;
            case State.SPIN:
                transform.Rotate(0, spinAttackRotationSpeed * Time.deltaTime, 0);
                transform.position = Vector3.MoveTowards(transform.position, spinAttackEndPoint, spinAttackMovementSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, spinAttackEndPoint) < 0.1f)
                {
                    swirlVFX.SetActive(false);
                    SetState(State.IDLE);
                }
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
        southCube.OnEndPointReached -= OnProjectileReachEnd;
        eastCube.OnEndPointReached -= OnProjectileReachEnd;
        westCube.OnEndPointReached -= OnProjectileReachEnd;

        heart.OnHeartHit -= OnDamagedEvent;
        heart.OnHeartDestroyed -= OnHeartDestroyedEvent;
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spinAttackRange);
    }

    public void SetState(State nextState)
    {
        currentState = nextState;

        switch (currentState)
        {
            case State.IDLE:
                OnChangeToIdleState?.Invoke();
                break;
            case State.CHASE:
                OnChangeToChaseState?.Invoke();
                break;
            case State.FIREBALL:
                OnChangeToFireballState?.Invoke();
                shot = true;
                sRoute.UpdateTarget();
                southCube.enabled = true;
                break;
            case State.MULTIPLE_FIREBALL:
                OnChangeToMultipleFireballState?.Invoke();
                southHitPlayer = false;
                shot = false;
                sRoute.UpdateTarget();
                eRoute.UpdateTarget();
                wRoute.UpdateTarget();
                southCube.enabled = true;
                eastCube.enabled = true;
                westCube.enabled = true;
                break;
            case State.SPIN:
                OnChangeToSpinState?.Invoke();
                swirlVFX.SetActive(true);
                spinAttackEndPoint = player.position + GetDirectionToPlayer() * 5f;
                spinAttackEndPoint.y = player.position.y;
                break;
            case State.READY_TO_ATTACK:
                break;
            case State.DEAD:
                Destroy(gameObject);
                break;
        }
    }

    private void OnProjectileReachEnd(bool isPlayerHit)
    {
        switch (currentState)
        {
            case State.FIREBALL:
                if (isPlayerHit)
                    southHitPlayer = true;
                else
                    southHitPlayer = false;
                SetState(State.IDLE);
                break;
            case State.MULTIPLE_FIREBALL:
                SetState(State.CHASE);
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
}
