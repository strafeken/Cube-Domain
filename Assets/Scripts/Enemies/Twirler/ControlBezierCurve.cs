using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBezierCurve : MonoBehaviour
{
    public Transform[] controlPoints;
    private Vector3 gizmosPosition;

    private Transform player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();    
    }

    public void UpdateTarget()
    {
        controlPoints[3].position = player.transform.position;
    }

    void OnDrawGizmosSelected()
    {
        for(float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.1f);
        }

        Gizmos.DrawLine(controlPoints[0].position, controlPoints[1].position);
        Gizmos.DrawLine(controlPoints[2].position, controlPoints[3].position);
    }
}
