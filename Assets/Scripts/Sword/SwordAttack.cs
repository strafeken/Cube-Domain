using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject point;

    public LayerMask layerMask;

    List<Transform> swordWaypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWaypointIndex;
    private float moveSpeed = 10f;
    private int numOfWaypoints = 0;

    public event Action OnAttackStart;
    public event Action OnAttackEnd;

    private SwordRendererController swordRendererController;

    public Transform player;
    public float swirlRadius = 3f;

    void Awake()
    {
        swordRendererController = GameObject.FindGameObjectWithTag("SwordRenderer").GetComponent<SwordRendererController>();
    }

    void Update()
    {
        if (swordRendererController.swordHasFormed && Input.GetMouseButtonDown(0))
            PlaceWaypoint();

        if (Input.GetMouseButtonDown(2) && numOfWaypoints > 1)
        {
            lastWaypointIndex = numOfWaypoints - 1;
            transform.position = swordWaypoints[0].position;
            targetWaypoint = swordWaypoints[targetWaypointIndex];
            StartCoroutine(MoveTowards());
        }

        // Swirl Skill
        if(Input.GetKeyDown(KeyCode.Q))
        {
            CircleWaypoint();
        }
    }

    void CircleWaypoint()
    {
        ResetValues();
        for (int i = 0; i < 8; ++i)
        {
            float angle = i * Mathf.PI * 2f / 8;
            Vector3 newPos = new Vector3(player.position.x + Mathf.Cos(angle) * swirlRadius, player.position.y, player.position.z + Mathf.Sin(angle) * swirlRadius);
            GameObject waypoint = Instantiate(point, newPos, Quaternion.identity);
            swordWaypoints.Add(waypoint.transform);
            ++numOfWaypoints;
        }
    }

    void PlaceWaypoint()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out RaycastHit hit, float.MaxValue, layerMask))
        {
            GameObject waypoint = Instantiate(point, hit.point, Quaternion.identity);
            swordWaypoints.Add(waypoint.transform);
            ++numOfWaypoints;
        }
    }

    IEnumerator MoveTowards()
    {
        OnAttackStart?.Invoke();
        while (targetWaypointIndex < numOfWaypoints)
        {
            float distanceToMove = moveSpeed * Time.deltaTime;
            float distance = Vector3.Distance(transform.position, targetWaypoint.position);
            CheckDistanceToWaypoint(distance);
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, distanceToMove);
            yield return null;
        }
        ResetValues();
    }

    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            UpdateTargetWaypoint();
        }
    }

    void UpdateTargetWaypoint()
    {
        ++targetWaypointIndex;
        if (targetWaypointIndex > lastWaypointIndex)
            return;
        targetWaypoint = swordWaypoints[targetWaypointIndex];
    }

    void ResetValues()
    {
        targetWaypointIndex = 0;
        for (int i = 0; i < numOfWaypoints; ++i)
        {
            Destroy(swordWaypoints[i].gameObject);
        }
        swordWaypoints.Clear();
        numOfWaypoints = 0;
        OnAttackEnd?.Invoke();
    }
}
