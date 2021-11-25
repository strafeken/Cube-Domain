using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneBodyMovement : MonoBehaviour
{
    private float rollSpeed = 50f;
    private bool isMoving;

    public Transform realBody;

    void Update()
    {

        if (isMoving)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            StartCoroutine(Roll(Vector3.right));
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            StartCoroutine(Roll(Vector3.left));
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            StartCoroutine(Roll(Vector3.forward));
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            StartCoroutine(Roll(Vector3.back));
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
            realBody.position = transform.position;
            remainingAngle -= rotationAngle;
            yield return null;
        }


        isMoving = false;
    }
}
