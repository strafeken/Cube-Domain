using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PillarCollision : MonoBehaviour
{
    public Action OnCollision;

    void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke();
    }
}
