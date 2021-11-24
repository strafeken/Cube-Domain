using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealisticSwordAttack : MonoBehaviour
{
    private Transform target;
    public GameObject targetPrefab;
    public LayerMask layerMask;
    public float moveSpeed = 10f;
    private float minDistance = 0.01f;

    private SwordRendererController swordRendererController;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public Transform player;

    public float maxIdleTime = 2f;
    private bool isIdleTimerRunning;

    private bool isSwinging;

    private float rotationSpeed = 30f;

    public bool isFirstSwing = true;

    [SerializeField] private GameObject blocked;
    [SerializeField] private GameObject notBlocked;
    [SerializeField] private GameObject stab;

    void Awake()
    {
        //swordRendererController = GameObject.FindGameObjectWithTag("SwordRenderer").GetComponent<SwordRendererController>();
    }

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (/*swordRendererController.swordHasFormed && */Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            if (blocked.activeInHierarchy)
                return;

            /*            if (isFirstSwing && stab.activeInHierarchy)
                            return;*/

            if (PlaceTarget())
            {
                if (stab.activeInHierarchy)
                {
                    transform.forward = Camera.main.transform.forward;
                    StartCoroutine("Stab");
                }
                else
                {
                    CalibrateSwordRotation();
                    DetermineTargetPositionRelativeToSword();
                    if (isSwinging)
                    {
                        StopCoroutine("Swing");
                        isSwinging = false;
                    }
                    StartCoroutine("Swing");
                }
            }
        }
    }

    private void ResetSword()
    {
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
        isFirstSwing = true;
    }

    void CalibrateSwordRotation()
    {
        //StartCoroutine("FirstSwingRotation");
        transform.forward = target.forward;
    }

    IEnumerator FirstSwingRotation()
    {
        Quaternion rot = Quaternion.LookRotation(target.forward);
        while (transform.rotation != rot)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void DetermineTargetPositionRelativeToSword()
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.up, dirToTarget);

        // Left
        if (Vector3.Dot(transform.right, dirToTarget) < 0)
        {
            // Top
            if (angle < 90f)
                transform.RotateAround(transform.position, transform.forward, 180 + angle);
            // Bottom
            else
                transform.RotateAround(transform.position, transform.forward, 180 + angle);
        }
        // Right
        else
        {
            if (angle < 90f)
                transform.RotateAround(transform.position, transform.forward, 180 - angle);
            // Bottom
            else
                transform.RotateAround(transform.position, transform.forward, 180 - angle);
        }
    }

    private bool PlaceTarget()
    {
        if (Physics.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition),
            out RaycastHit hit,
            float.MaxValue,
            layerMask))
        {
            if (target)
                Destroy(target.gameObject);

            GameObject obj = Instantiate(targetPrefab, hit.point, Quaternion.identity, player);
            target = obj.transform;
            target.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }
        return false;
    }

    private IEnumerator Swing()
    {
        isSwinging = true;

        if (isIdleTimerRunning)
        {
            StopCoroutine("IdleTimer");
            isIdleTimerRunning = false;
        }

        float distanceToTarget = float.MaxValue;
        float distanceToMove = moveSpeed * Time.deltaTime;

        while (distanceToTarget > minDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, distanceToMove);
            distanceToTarget = (transform.position - target.position).magnitude;
            if (isFirstSwing)
                isFirstSwing = false;
            yield return null;
        }

        StartCoroutine("IdleTimer");
        isSwinging = false;
    }

    IEnumerator IdleTimer()
    {
        isIdleTimerRunning = true;
        yield return new WaitForSeconds(maxIdleTime);
        ResetSword();
        isIdleTimerRunning = false;
    }

    // Limit stab range to be infront of player
    private IEnumerator Stab()
    {
        if (isIdleTimerRunning)
        {
            StopCoroutine("IdleTimer");
            isIdleTimerRunning = false;
        }

        Vector3 initialPos = transform.position;
        Vector3 pullPos = initialPos - transform.forward * 0.5f;
        float distanceToTarget = float.MaxValue;
        float distanceToMove = moveSpeed * Time.deltaTime;

        while (distanceToTarget > minDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, pullPos, distanceToMove);
            distanceToTarget = (transform.position - pullPos).magnitude;
            yield return null;
        }

        distanceToTarget = float.MaxValue;
        while (distanceToTarget > minDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, distanceToMove);
            distanceToTarget = (transform.position - target.position).magnitude;
            yield return null;
        }
    }

    private void MoveBack()
    {

    }

    private void MoveForward()
    {

    }
}
