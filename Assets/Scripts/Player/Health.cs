using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 10;
    private int maxHealth;
    private AudioSource sfx;

    public event Action OnDamaged;
    public event Action OnDeath;

    void Awake()
    {
        sfx = GetComponent<AudioSource>();    
    }

    void Start()
    {
        maxHealth = health;
    }

    public void DealDamage()
    {
        --health;
        OnDamaged?.Invoke();

        AudioSource.PlayClipAtPoint(sfx.clip, transform.position);

        if (health < 1)
            OnDeath?.Invoke();
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
