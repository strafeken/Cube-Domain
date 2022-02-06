using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBarOfFortune : MonoBehaviour
{
    [SerializeField] private int health = 5;

    [SerializeField] private GameObject sparksVFX;

    [SerializeField] private AudioSource sfxHit;
    [SerializeField] private AudioSource sfxBreak;


    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Blade")
            return;

        GameObject vfx = Instantiate(sparksVFX, other.transform.position, Quaternion.identity);
        Destroy(vfx, 1f);
        sfxHit.Play();

        --health;

        if (health < 1)
        {
            AudioSource.PlayClipAtPoint(sfxBreak.clip, transform.position);
            Destroy(gameObject);
        }
    }
}
