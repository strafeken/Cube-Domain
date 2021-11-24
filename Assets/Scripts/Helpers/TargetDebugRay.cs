using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDebugRay : MonoBehaviour
{
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up * 10f, Color.yellow);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.blue);
    }
}
