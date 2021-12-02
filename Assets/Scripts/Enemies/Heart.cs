using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heart : MonoBehaviour
{
    [SerializeField] private int health = 3;
    public event Action OnHeartHit;
    public event Action OnHeartDestroyed;

    void OnTriggerEnter(Collider other)
    {
        --health;
        
        OnHeartHit?.Invoke();

        if (health < 1)
        {
            OnHeartDestroyed?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public int GetCurrentHealth()
    {
        return health;
    }
}
