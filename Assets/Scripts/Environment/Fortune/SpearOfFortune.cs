using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearOfFortune : MonoBehaviour
{
    private Transform player;
    private Health playerHP;
    [SerializeField] private Transform tip;
    [SerializeField] private Transform tail;

    private Renderer mat;

    [SerializeField] private float timeTillDissolve = 0.5f;

    private IEnumerator coroutine;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHP = player.gameObject.GetComponent<Health>();
        mat = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHP.DealDamage();
        }
    }

    public void Shoot(float moveSpeed)
    {
        coroutine = MoveToTarget(moveSpeed);
        StartCoroutine(coroutine);
    }

    private IEnumerator MoveToTarget(float moveSpeed)
    {
        while (tail.position.y < 0f)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
            yield return null;
        }

        float alpha = 0f;
        while(alpha < 1f)
        {
            alpha += Time.deltaTime / timeTillDissolve;
            mat.material.SetFloat("Alpha_Ref", alpha);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
