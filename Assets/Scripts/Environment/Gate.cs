using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private float openingSpeed = 1f;

    public void OpenGate()
    {
        StartCoroutine(Opening());
    }

    public void CloseGate()
    {
        StartCoroutine(Closing());
    }

    private IEnumerator Opening()
    {
        while(transform.position.y < 5f)
        {
            transform.position += Vector3.up * openingSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Closing()
    {
        while (transform.position.y > 0f)
        {
            transform.position -= Vector3.up * openingSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
