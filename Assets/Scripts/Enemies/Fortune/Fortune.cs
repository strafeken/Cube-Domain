using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fortune : Enemy
{
    public enum State
    {
        CHASE,
        MISSILE,
        CAGE
    }

    private GameObject hearts;
    private GameObject h1;
    private GameObject h2;
    private GameObject h3;
    private GameObject h4;
    private GameObject h5;
    private GameObject h6;

    [SerializeField] private GameObject skills;
    [Header("Pillars")]
    private Transform[] pillars = new Transform[6];
    [SerializeField] private float impaleSpeed = 10f;

    private State currentState;

    public Transform body;

    [Header("Missile")]
    [SerializeField] private Transform missilePosition;
    [SerializeField] private float missileSpeed = 5f;

    [Header("Cage")]
    [SerializeField] private float cageRadius = 2f;
    [SerializeField] private float risingSpeed = 1f;
    [SerializeField] private Transform cagePosition;

    private FortuneBodyMovement bodyMovement;

    void Awake()
    {
        Transform heartGroup = transform.Find("Hearts");
        hearts = heartGroup.gameObject;
        h1 = heartGroup.Find("1H").gameObject;
        h2 = heartGroup.Find("2H").gameObject;
        h3 = heartGroup.Find("3H").gameObject;
        h4 = heartGroup.Find("4H").gameObject;
        h5 = heartGroup.Find("5H").gameObject;
        h6 = heartGroup.Find("6H").gameObject;

        Transform skillsGroup = transform.Find("Skills");
        for(int i = 0; i < 6; ++i)
            pillars[i] = skillsGroup.Find("Pillar" + (i + 1));

        bodyMovement = GetComponent<FortuneBodyMovement>();
    }

    protected override void Start()
    {
        base.Start();

        bodyMovement.OnSideChanged += OnSideChangedEvent;

        SetState(State.CHASE);
    }

    void OnDisable()
    {
        bodyMovement.OnSideChanged -= OnSideChangedEvent;
    }

    protected override void Update()
    {
        hearts.transform.position = transform.position + Vector3.up * 2;
        hearts.transform.LookAt(new Vector3(player.position.x, hearts.transform.position.y, player.position.z));

        skills.transform.position = transform.position - Vector3.up;

        switch (currentState)
        {
            case State.CHASE:
                break;
        }
    }

    public void SetState(State nextState)
    {
        currentState = nextState;

        switch(currentState)
        {
            case State.CHASE:
                GetComponent<FortuneBodyMovement>().enabled = true;
                //ResetPillars();
                break;
            case State.MISSILE:
                GetComponent<FortuneBodyMovement>().enabled = false;
                ResetPillars();
                //StartCoroutine("Missile");
                for(int i = 0; i < 5; ++i)
                {
                    pillars[i].GetComponent<PillarMissile>().FaceTarget(((player.position - Vector3.down) - pillars[i].position).normalized);
                }
                break;
            case State.CAGE:
                GetComponent<FortuneBodyMovement>().enabled = false;
                ResetPillars();
                //StartCoroutine("Cage");
                break;
        }
    }

    private IEnumerator Impale()
    {
        float distance = float.MaxValue;
        while(distance > 2f)
        {
            //pillar.transform.position = Vector3.MoveTowards(pillar.transform.position, player.position, impaleSpeed * Time.deltaTime);
            //distance = Vector3.Distance(player.position, pillar.transform.position);
            yield return null;
        }
        //pillar.transform.localPosition = initialPillarPosition;
    }

    private IEnumerator Missile()
    {
        for (int i = 0; i < 5; ++i)
        {
            float angle = i * Mathf.PI * 2f / 5;
            Vector3 newPos = new Vector3(transform.position.x + Mathf.Cos(angle) * cageRadius, missilePosition.position.y + Mathf.Sin(angle) * cageRadius, transform.position.z);
            pillars[i].position = newPos;
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 5; ++i)
        {
            pillars[i].GetComponent<PillarMissile>().Move(player.position + (Vector3.down), missileSpeed);
            yield return new WaitForSeconds(0.5f);
        }

        SetState(State.CHASE);
    }

    private IEnumerator Cage()
    {
        for (int i = 0; i < 6; ++i)
        {
            float angle = i * Mathf.PI * 2f / 6;
            Vector3 newPos = new Vector3(player.position.x + Mathf.Cos(angle) * cageRadius, pillars[i].position.y, player.position.z + Mathf.Sin(angle) * cageRadius);
            pillars[i].position = newPos;
        }

        while (pillars[5].position.y < cagePosition.position.y)
        {
            for(int i = 0; i < 6; ++i)
            {
                Vector3 currentPosition = pillars[i].position;
                pillars[i].position = new Vector3(currentPosition.x, currentPosition.y + risingSpeed * Time.deltaTime, currentPosition.z);
            }
            yield return null;
        }
    }

    private void ResetPillars()
    {
        for(int i = 0; i <6; ++i)
        {
            pillars[i].rotation = Quaternion.identity;
            pillars[i].position = new Vector3(transform.position.x, -5, transform.position.z);
        }
    }

    private void OnSideChangedEvent(string name)
    {
        h1.SetActive(false);
        h2.SetActive(false);
        h3.SetActive(false);
        h4.SetActive(false);
        h5.SetActive(false);
        h6.SetActive(false);

        switch (name)
        {
            case "Num1":
                h1.SetActive(true);
                break;
            case "Num2":
                h2.SetActive(true);
                break;
            case "Num3":
                h3.SetActive(true);
                break;
            case "Num4":
                h4.SetActive(true);
                break;
            case "Num5":
                h5.SetActive(true);
                break;
            case "Num6":
                h6.SetActive(true);
                break;
        }

        if (h1.activeInHierarchy)
        {
            //StartCoroutine("Impale");
        }
        else if (h2.activeInHierarchy)
        {

        }
        else if (h3.activeInHierarchy)
        {

        }
        else if (h4.activeInHierarchy)
        {

        }
        else if (h5.activeInHierarchy)
        {
            //SetState(State.MISSILE);
        }
        else if (h6.activeInHierarchy)
        {
            //SetState(State.CAGE);
        }
    }
}
