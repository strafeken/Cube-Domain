using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FortuneBodyMovement : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 10f;

    [SerializeField] private Transform[] sides;

    private List<float> listOfDistance = new List<float>();

    private bool isMoving;

    [SerializeField] private Transform player;

    public Transform body;

    private Fortune fortune;

    [SerializeField] private float rollCooldown = 4f;
    private float rollCooldownTimer = 0f;

    public Action<string> OnSideChanged;

    void Awake()
    {
        fortune = GetComponent<Fortune>();    
    }

    void Start()
    {
        DetermineSideFacingUp();
    }

    void Update()
    {
        if (isMoving)
            return;

        rollCooldownTimer += Time.deltaTime;
        if (rollCooldownTimer > rollCooldown)
        {
            FindBestDirection();
            rollCooldownTimer = 0f;
        }
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

        Vector3 rotationCenter = body.transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(rollSpeed * Time.deltaTime, remainingAngle);
            body.transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            transform.position = body.transform.position;
            remainingAngle -= rotationAngle;
            yield return null;
        }

        isMoving = false;

        DetermineSideFacingUp();
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
        Debug.Log(highestFilling.name);
        OnSideChanged?.Invoke(highestFilling.name);
    }
}
