using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fortune : Enemy
{
    public enum State
    {
        CHASE
    }

    [SerializeField] private float rollSpeed = 10f;

    [SerializeField] private Transform[] sides;

    private List<float> listOfDistance = new List<float>();

    bool isMoving;

    [SerializeField] private GameObject hearts;
    [SerializeField] private GameObject h1;
    [SerializeField] private GameObject h2;
    [SerializeField] private GameObject h3;
    [SerializeField] private GameObject h4;
    [SerializeField] private GameObject h5;
    [SerializeField] private GameObject h6;


    [SerializeField] private GameObject skills;
    [SerializeField] private GameObject pillar;
    [SerializeField] private float impaleSpeed = 10f;

    private Vector3 initialPillarPosition;

    private float rollCooldown = 4f;
    public float rollCooldownTimer = 0f;

    private State currentState;

    protected override void Start()
    {
        base.Start();

        DetermineSideFacingUp();

        initialPillarPosition = pillar.transform.localPosition;

       // StartCoroutine("Impale");
        SetState(State.CHASE);
        //SetState(State.IDLE);
    }

    protected override void Update()
    {
        hearts.transform.position = transform.position + Vector3.up * 2;
        hearts.transform.LookAt(new Vector3(player.position.x, hearts.transform.position.y, player.position.z));

        skills.transform.position = transform.position - Vector3.up;

        switch (currentState)
        {
            case State.CHASE:
                if (isMoving)
                    return;

                rollCooldownTimer += Time.deltaTime;
                if(rollCooldownTimer > rollCooldown)
                {
                    FindBestDirection();
                    rollCooldownTimer = 0f;
                }


                break;
        }

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //    StartCoroutine(Roll(Vector3.right));
        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    StartCoroutine(Roll(Vector3.left));
        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //    StartCoroutine(Roll(Vector3.forward));
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //    StartCoroutine(Roll(Vector3.back));
    }

    public void SetState(State nextState)
    {
        currentState = nextState;
    }

    private void FindBestDirection()
    {
        listOfDistance.Clear();

        for (int i = 0; i < 4; ++i)
        {
            switch (i)
            {
                case 0:
                    ProjectMovement(Vector3.right);
                    break;
                case 1:
                    ProjectMovement(Vector3.left);
                    break;
                case 2:
                    ProjectMovement(Vector3.forward);
                    break;
                case 3:
                    ProjectMovement(Vector3.back);
                    break;
            }
        }

        float minVal = listOfDistance.Min();
        int index = listOfDistance.IndexOf(minVal);

        switch (index)
        {
            case 0:
                StartCoroutine(Roll(Vector3.right));
                break;
            case 1:
                StartCoroutine(Roll(Vector3.left));
                break;
            case 2:
                StartCoroutine(Roll(Vector3.forward));
                break;
            case 3:
                StartCoroutine(Roll(Vector3.back));
                break;
        }
    }

    private void DetermineSideFacingUp()
    {
        //sides = sides.OrderBy(sides => sides.transform.position.y).ToArray();
        GameObject highestFilling = null;
        float highestPosition = float.MinValue;
        for (int f = 0; f < sides.Length; f++)
        {
            float thisY = sides[f].transform.position.y; //cache this, because calculating it twice is also slower than need be
            if (thisY > highestPosition)
            {
                highestPosition = thisY;
                highestFilling = sides[f].gameObject;
            }
        }
        h1.SetActive(false);
        h2.SetActive(false);
        h3.SetActive(false);
        h4.SetActive(false);
        h5.SetActive(false);
        h6.SetActive(false);

        switch (highestFilling.name)
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
            StartCoroutine("Impale");
        }
        else if(h2.activeInHierarchy)
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

        }
        else if (h6.activeInHierarchy)
        {

        }
    }

    private IEnumerator Impale()
    {
        float distance = float.MaxValue;
        while(distance > 2f)
        {
            pillar.transform.position = Vector3.MoveTowards(pillar.transform.position, player.position, impaleSpeed * Time.deltaTime);
            distance = Vector3.Distance(player.position, pillar.transform.position);
            yield return null;
        }
        pillar.transform.localPosition = initialPillarPosition;
    }

    private void ProjectMovement(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction;
        float distanceToPlayer = (player.position - newPosition).magnitude;
        listOfDistance.Add(distanceToPlayer);
    }


    private IEnumerator Roll(Vector3 direction)
    {
        isMoving = true;

        float remainingAngle = 90;

        Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(rollSpeed * Time.deltaTime, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        isMoving = false;

        DetermineSideFacingUp();
    }
}
