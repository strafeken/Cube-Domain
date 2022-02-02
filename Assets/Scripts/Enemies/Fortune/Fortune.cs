using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fortune : Enemy
{
    public enum State
    {
        CHASE,
        IMPALE,
        LASER,
        MISSILE,
        CAGE
    }

    private GameObject hearts;


    private State currentState;

    public Transform body;

    [Header("Spear")]
    [SerializeField] private GameObject spear;
    [SerializeField] private float spearSpeed = 10f;
    [SerializeField] private float spearTravelTime = 1.5f;

    [Header("Laser")]
    [SerializeField] private GameObject laser;
    [SerializeField] private float laserDuration = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    private float laserTimer = 0f;

    [Header("Missile")]
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform missilePosition;
    [SerializeField] private float missileSpeed = 5f;
    [SerializeField] private float timeUntilShootingStarts = 2f;
    [SerializeField] private float shootingCooldown = 1f;

    [Header("Cage")]
    [SerializeField] private GameObject cage;
    [SerializeField] private float cageRadius = 2f;
    [SerializeField] private float risingSpeed = 1f;
    [SerializeField] private Transform cageEndPosition;

    public Action OnUsedSkill;

    void Awake()
    {
        hearts = transform.Find("Hearts").gameObject;    
    }

    protected override void Start()
    {
        base.Start();

        //StartCoroutine(Laser());
        //laser.SetActive(true);
        //StartCoroutine(Missile());
        //StartCoroutine(Cage());
        SetState(State.CHASE);
    }

    protected override void Update()
    {
        hearts.transform.position = transform.position + Vector3.up * 1.5f;
        //hearts.transform.LookAt(new Vector3(player.position.x, hearts.transform.position.y, player.position.z));

        switch (currentState)
        {
            case State.CHASE:
                break;
            case State.LASER:
                break;
        }
    }

    public void SetState(State nextState)
    {
        currentState = nextState;

        switch (currentState)
        {
            case State.CHASE:
                GetComponent<FortuneBodyMovement>().enabled = true;
                break;
            case State.IMPALE:
                GetComponent<FortuneBodyMovement>().enabled = false;
                StartCoroutine("Impale");
                break;
            case State.LASER:
                GetComponent<FortuneBodyMovement>().enabled = false;
                StartCoroutine("Laser");
                break;
            case State.MISSILE:
                GetComponent<FortuneBodyMovement>().enabled = false;
                StartCoroutine("Missile");
                break;
            case State.CAGE:
                GetComponent<FortuneBodyMovement>().enabled = false;
                StartCoroutine("Cage");
                break;
        }
    }

    private IEnumerator Impale()
    {
        Vector3 spawnPoint = player.position + player.forward * 10;
        GameObject spearObject = Instantiate(spear, new Vector3(spawnPoint.x, -2.5f, spawnPoint.z), Quaternion.identity);
        spearObject.transform.up = (player.position - spearObject.transform.position).normalized;
        spearObject.GetComponent<SpearOfFortune>().Shoot(player.position + (Vector3.up * 0.5f), spearSpeed, spearTravelTime);

        yield return new WaitForSeconds(1f);

        SetState(State.CHASE);
    }

    private IEnumerator Laser()
    {
        laser.SetActive(true);
        laser.GetComponent<LaserOfFortune>().Shoot();

        while(laserTimer < laserDuration)
        {
            laser.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            laserTimer += Time.deltaTime;
            yield return null;
        }

        laser.SetActive(false);

        SetState(State.CHASE);
    }

    private IEnumerator Missile()
    {
        List<GameObject> missiles = new List<GameObject>();

        for (int i = 0; i < 5; ++i)
        {
            float angle = i * Mathf.PI * 2f / 5;
            Vector3 pos = new Vector3(transform.position.x + Mathf.Cos(angle) * cageRadius, missilePosition.position.y + Mathf.Sin(angle) * cageRadius, transform.position.z);
            missiles.Add(Instantiate(missile, pos, Quaternion.identity));
        }

        yield return new WaitForSeconds(timeUntilShootingStarts);

        for (int i = 0; i < 5; ++i)
        {
            missiles[i].GetComponent<MissileOfFortune>().Shoot(player.position + (Vector3.down * 0.5f), missileSpeed);
            yield return new WaitForSeconds(shootingCooldown);
        }

        yield return new WaitForSeconds(1f);
        
        SetState(State.CHASE);
    }

    private IEnumerator Cage()
    {
        GameObject go = Instantiate(cage, new Vector3(player.position.x, -2.5f, player.position.z), Quaternion.identity);
        Transform cageTransform = go.GetComponent<Transform>();
        
        while(cageTransform.position.y < cageEndPosition.position.y)
        {
            cageTransform.position += Vector3.up * risingSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(10f);

        SetState(State.CHASE);
    }
}
