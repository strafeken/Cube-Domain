using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBarOfFortune : MonoBehaviour
{
    [SerializeField] private int health = 5;

    [SerializeField] private GameObject sparksVFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Blade")
            return;

        GameObject vfx = Instantiate(sparksVFX, other.transform.position, Quaternion.identity);
        Destroy(vfx, 1f);

        --health;

        if (health < 1)
        {
            Destroy(gameObject);
        }
    }
}
