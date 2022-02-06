using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCollision : MonoBehaviour
{
    private Health playerHP;

    void Awake()
    {
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHP.DealDamage();
        }
    }
}
