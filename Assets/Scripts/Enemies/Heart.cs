using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heart : MonoBehaviour
{
    [SerializeField] private int health = 3;
    public event Action OnHeartHit;
    public event Action OnHeartDestroyed;

    [SerializeField] private GameObject sparksVFX;

    private AudioSource sfx;

    void Awake()
    {
        sfx = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject vfx = Instantiate(sparksVFX, other.transform.position, Quaternion.identity);
        Destroy(vfx, 1f);

        AudioSource.PlayClipAtPoint(sfx.clip, transform.position);

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
