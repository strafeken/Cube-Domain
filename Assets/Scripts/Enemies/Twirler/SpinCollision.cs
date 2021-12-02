using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCollision : MonoBehaviour
{
    private Health playerHealth;

    private Twirler twirler;

    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        twirler = GetComponentInParent<Twirler>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (twirler.GetCurrentState() != Twirler.State.SPIN)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.DealDamage();
        }
    }
}
