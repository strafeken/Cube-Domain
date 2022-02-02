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

    [SerializeField] private Transform tip;
    [SerializeField] private Transform tail;

    [SerializeField] private GameObject groundCrack;

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

    public void Shoot(float moveSpeed)
    {
        isShot = true;
        StartCoroutine(MoveToTarget(moveSpeed));
    }

    private IEnumerator FacePlayer()
    {
        while (!isShot)
        {
            transform.up = (player.position - transform.position).normalized;
            yield return null;
        }
    }

    private IEnumerator MoveToTarget(float moveSpeed)
    {
        bool isGroundHit = false;
        while (tail.transform.position.y > 0.01f)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;

            if(tip.transform.position.y < 0.01f)
            {
                if(!isGroundHit)
                {
                    Instantiate(groundCrack, new Vector3(tip.position.x, 0.01f, tip.position.z), Quaternion.identity);
                    isGroundHit = true;
                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
