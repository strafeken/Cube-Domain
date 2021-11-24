using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float height = 1f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.parent.position.y + (Mathf.Sin(Time.time * speed) * height), transform.position.z);
    }
}
