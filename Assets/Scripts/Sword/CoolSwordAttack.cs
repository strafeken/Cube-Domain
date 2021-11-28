using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolSwordAttack : MonoBehaviour
{
    private Transform target;
    public GameObject targetPrefab;
    public LayerMask layerMask;
    public Transform player;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 defaultPosition = new Vector3(0.4f, 0.28f, 1f);

    private Vector3 firstPosition;
    private Quaternion firstRotation;

    [SerializeField] private float swingSpeed = 1f;

    public Transform[] controlPoints;
    private Vector3 gizmosPosition;

    [SerializeField] private Transform swordTip;

    private Vector3 swingDirection;

    private Camera playerCamera;

    [SerializeField] private Transform raycastPoint;
    [SerializeField] private Transform raycastPointDirection;


    private int previousSide = -1;

    [SerializeField] private MouseControl mouseControl;

    enum Side
    {
        LEFT,
        RIGHT,
        DOWN,
        UP
    }

    void Awake()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Start()
    {
        // Set position
        transform.localPosition = new Vector3(0.3f, 0f, 0.5f);
        transform.localRotation = Quaternion.Euler(30f, 0f, 0f);

        firstPosition = new Vector3(0.05f, 0.35f, 0.9f);
        firstRotation = Quaternion.Euler(0, -5f, -70f);

        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTarget())
            {
                swingDirection = (target.position - swordTip.position).normalized;
                Vector3 hitPoint = FindPointOfIntersectionWithScreenBorder(swordTip.position, swingDirection);
                target.position = hitPoint;

                controlPoints[0].position = transform.position;

                Vector3 absoluteDistance = target.position - swordTip.position;
                Vector3 endPosition = transform.position + absoluteDistance;

                controlPoints[3].position = endPosition;

                Vector3 midPoint = (controlPoints[0].position + controlPoints[3].position) * 0.5f;
                controlPoints[1].position = midPoint;
                controlPoints[2].position = midPoint;

                StartCoroutine("Swing");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.05f);
        }
    }

    private IEnumerator Swing()
    {
        mouseControl.enabled = false;
        Vector3 p0 = controlPoints[0].position;
        Vector3 p1 = controlPoints[1].position;
        Vector3 p2 = controlPoints[2].position;
        Vector3 p3 = controlPoints[3].position;
        float t = 0;
        Vector3 nextPosition;
        while (t < 1)
        {
            t += Time.deltaTime * swingSpeed;

            nextPosition = Mathf.Pow(1 - t, 3) * p0 +
                3 * Mathf.Pow(1 - t, 2) * t * p1 +
                3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                Mathf.Pow(t, 3) * p3;

            transform.position = nextPosition;
            yield return null;
        }
        mouseControl.enabled = true;

        //transform.localPosition = initialPosition;
        //transform.localRotation = initialRotation;
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

            GameObject obj = Instantiate(targetPrefab, hit.point - player.forward / 5f, Quaternion.identity, player);
            target = obj.transform;
            target.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }
        return false;
    }

    private Vector3 FindPointOfIntersectionWithScreenBorder(Vector3 origin, Vector3 direction)
    {
        Ray ray = new Ray(origin, direction);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        Vector3 hitPoint = Vector3.zero;

        // Raycast against the plane
        if (planes[FindClosestPlane()].Raycast(ray, out float distance))
        {
            hitPoint = ray.GetPoint(distance);
        }

        hitPoint -= swingDirection * 0.2f;

        return hitPoint;
    }

    // 1. Find the side of the view frustum that the sword is going to swing to
    // 2. Find the point of intersection with that side
    // 3. Swing to it
    private int FindClosestPlane()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = raycastPoint.position.z - Mathf.Epsilon;

        Vector3 dirToEdge = (playerCamera.ScreenToWorldPoint(mousePos) - raycastPoint.position).normalized;
        Ray ray = new Ray(raycastPoint.position, dirToEdge);

        //Vector3 dirToEdge = (playerCamera.ScreenToWorldPoint(mousePos) - swordTip.position).normalized;
        //Ray ray = new Ray(swordTip.position, dirToEdge);

        //Debug.DrawRay(swordTip.position, dirToEdge, Color.green, 10f);
        //Debug.DrawRay(raycastPoint.position, dirToEdge, Color.green, 10f);

        float currentMinDistance = float.MaxValue;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);

        int nearestSide = -1;

        for (int i = 0; i < 4; ++i)
        {
            if (planes[i].Raycast(ray, out float distance))
            {
                if (distance < currentMinDistance)
                {
                    currentMinDistance = distance;
                    nearestSide = i;
                }
            }
        }

        if (previousSide == nearestSide)
        {
            //dirToEdge = (playerCamera.ScreenToWorldPoint(mousePos) - (swordTip.position + swingDirection * 0.1f)).normalized;
            //ray = new Ray(swordTip.position + swingDirection * 0.1f, dirToEdge);

            nearestSide = -1;
            currentMinDistance = float.MaxValue;

            for (int i = 0; i < 4; ++i)
            {
                if (i == previousSide)
                    continue;

                if (planes[i].Raycast(ray, out float distance))
                {
                    if (distance < currentMinDistance)
                    {
                        currentMinDistance = distance;
                        nearestSide = i;
                    }
                }
            }
        }

        switch (nearestSide)
        {
            case -1:
                Debug.Log("Error");
                break;
            case 0:
                Debug.Log("Left 0");
                break;
            case 1:
                Debug.Log("Right 1");
                break;
            case 2:
                Debug.Log("Down 2");
                break;
            case 3:
                Debug.Log("Up 3");
                break;
            case 4:
                Debug.Log("Near 4");
                do
                {
                    nearestSide = Random.Range(0, 3);
                } while (nearestSide != previousSide);
                break;
        }

        if (nearestSide == -1)
            return 0;

        previousSide = nearestSide;

        return nearestSide;
    }
}
