using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearOfFortune : MonoBehaviour
{
    private Transform player;
    private Health playerHP;
    private Transform tip;

    private Renderer mat;

    [SerializeField] private float timeTillDissolve = 0.5f;

    private bool isCoroutineRunning = false;
    private IEnumerator coroutine;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHP = player.gameObject.GetComponent<Health>();
        tip = transform.Find("Tip").GetComponent<Transform>();
        mat = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHP.DealDamage();
            if(isCoroutineRunning)
            {
                StopCoroutine(coroutine);
                StartCoroutine("Dissolve");
            }
        }
    }

    public void Shoot(Vector3 target, float moveSpeed, float travelTime)
    {
        coroutine = MoveToTarget(target, moveSpeed, travelTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator MoveToTarget(Vector3 target, float moveSpeed, float travelTime)
    {
        isCoroutineRunning = true;

        // Sometimes passes through due to high movement speed
        while (Vector3.Distance(tip.position, target) > 0.1f)
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

        isCoroutineRunning = false;
        
        Destroy(gameObject);
    }

    private IEnumerator Dissolve()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / timeTillDissolve;
            mat.material.SetFloat("Alpha_Ref", alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
