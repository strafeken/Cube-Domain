using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{
    public float rotX, rotY, rotZ;
    public float moveScale;
    public Vector3 changeSpeed = Vector3.one;
    private Rigidbody rb;
    private Vector3 progression;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    private void UpdateAngularVelocity()
    {
        this.rb.angularVelocity = new Vector3(
            rotX * Mathf.Sin(progression.x),
            rotY * Mathf.Cos(progression.y),
            rotZ * (Mathf.Cos(progression.z) + Mathf.Sin(progression.z))
        );
    }

    void UpdatePosition()
    {
        Vector3 currentPosition = transform.localPosition;
        Vector3 magnetPosition = moveScale * new Vector3(
            rotX * Mathf.Sin(progression.x),
            rotY * Mathf.Cos(progression.y),
            rotZ * (Mathf.Cos(progression.z) + Mathf.Sin(progression.z))
        );
        Vector3 newPosition = Vector3.Lerp(currentPosition, magnetPosition, Time.deltaTime * 2);
        transform.localPosition = newPosition;
    }

    void Update()
    {
        UpdateAngularVelocity();
        UpdatePosition();
        progression += Time.deltaTime * changeSpeed;
    }
}
