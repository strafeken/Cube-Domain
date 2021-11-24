using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heart : MonoBehaviour
{
    public event Action OnHeartHit;

    void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        OnHeartHit?.Invoke();
    }
}
