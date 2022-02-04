using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCurveRoute : MonoBehaviour
{
    public Transform[] routes;
    private int routeToGo = 0;
    private float t = 0f;
    private Vector3 nextPosition;
    [SerializeField] private float speed = 0.5f;
    private Rigidbody rb;

    private IEnumerator MoveCoroutine;

    private TrailRenderer tr;

    public Action<bool> OnEndPointReached;
    private bool hitPlayer;

    private Health playerHealth;

    private AudioSource sfx;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        sfx = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        MoveCoroutine = GoByTheRoute(routeToGo);
        StartCoroutine(MoveCoroutine);
    }

    void OnTriggerEnter(Collider other)
    {
        // Because collision is still triggered even if this script is disabled
        if (!enabled)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            hitPlayer = true;
            StopCoroutine(MoveCoroutine);
            enabled = false;
            playerHealth.DealDamage();
        }
    }

    void OnDisable()
    {
        t = 0f;
        transform.position = routes[0].GetChild(0).position;
        tr.Clear();
        rb.velocity = new Vector3(0f, 0f, 0f);
        //rb.angularVelocity = new Vector3(0f, 0f, 0f);
        //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        OnEndPointReached?.Invoke(hitPlayer);
        hitPlayer = false;
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        sfx.Play();

        Vector3 p0 = routes[routeNumber].GetChild(0).position;
        Vector3 p1 = routes[routeNumber].GetChild(1).position;
        Vector3 p2 = routes[routeNumber].GetChild(2).position;
        Vector3 p3 = routes[routeNumber].GetChild(3).position;

        while (t < 1)
        {
            t += Time.deltaTime * speed;

            nextPosition = Mathf.Pow(1 - t, 3) * p0 +
                3 * Mathf.Pow(1 - t, 2) * t * p1 +
                3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                Mathf.Pow(t, 3) * p3;

            transform.position = nextPosition;

            yield return new WaitForEndOfFrame();
        }

        sfx.Stop();

        t = 0f;

        ++routeToGo;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
            enabled = false;
        }
    }
}
