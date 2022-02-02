using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearOfFortune : MonoBehaviour
{
    private Transform player;
    private Health playerHP;
    private Transform tip;
    [SerializeField] private GameObject groundCrack;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHP = player.gameObject.GetComponent<Health>();
        tip = transform.Find("Tip").GetComponent<Transform>();
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
        Instantiate(groundCrack, new Vector3(tip.position.x, 0.01f, tip.position.z), Quaternion.identity);
        Debug.Break();
        float timer = 0f;

        while (timer < travelTime)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
