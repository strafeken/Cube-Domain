using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearOfFortune : MonoBehaviour
{
    private Transform player;
    private Health playerHP;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHP = player.gameObject.GetComponent<Health>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHP.DealDamage();
        }
    }

    public void Shoot(Vector3 target, float moveSpeed, float travelTime)
    {
        StartCoroutine(MoveToTarget(target, moveSpeed, travelTime));
    }

    private IEnumerator MoveToTarget(Vector3 target, float moveSpeed, float travelTime)
    {
        float timer = 0f;

        while (timer < travelTime)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
