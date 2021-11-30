using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heart : MonoBehaviour
{
    [SerializeField] private int health = 3;
    public event Action OnHeartHit;

    void OnTriggerEnter(Collider other)
    {
        --health;
        if(health < 1)
        {
            gameObject.SetActive(false);
            OnHeartHit?.Invoke();
        }
    }
}
