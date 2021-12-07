using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BigSlimeCollision : MonoBehaviour
{
    public Action OnCollisionDetected;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnCollisionDetected?.Invoke();
        }
    }
}
