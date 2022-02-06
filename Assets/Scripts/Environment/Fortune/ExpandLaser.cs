using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();    
    }

    public void Expand(float beamLength, float expandSpeed)
    {
        StartCoroutine(Expanding(beamLength, expandSpeed));
    }

    private IEnumerator Expanding(float beamLength, float expandSpeed)
    {
        while (lineRenderer.GetPosition(1).z < beamLength)
        {
            lineRenderer.SetPosition(1, new Vector3(0, 0, lineRenderer.GetPosition(1).z + expandSpeed * Time.deltaTime));
            yield return null;
        }
    }
}
