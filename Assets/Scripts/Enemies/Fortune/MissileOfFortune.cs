using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Always faces the player before it moves
/// </summary>
public class MissileOfFortune : MonoBehaviour
{
    private Transform player;
    private Health playerHP;

    private bool isShot = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHP = player.gameObject.GetComponent<Health>();
    }

    void Start()
    {
        StartCoroutine("FacePlayer");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerHP.DealDamage();
        }
    }

    public void Shoot(Vector3 target, float moveSpeed)
    {
        isShot = true;
        StartCoroutine(MoveToTarget(target, moveSpeed));
    }

    private IEnumerator FacePlayer()
    {
        while (!isShot)
        {
            transform.up = (player.position - transform.position).normalized;
            yield return null;
        }
    }

    private IEnumerator MoveToTarget(Vector3 target, float moveSpeed)
    {
        Vector3 moveDirection = (target - transform.position).normalized;
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            //transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
