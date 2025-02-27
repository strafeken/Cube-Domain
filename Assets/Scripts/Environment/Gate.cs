using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private float openingSpeed = 1f;
    [SerializeField] private AudioSource movingSFX;
    [SerializeField] private AudioSource pulledSFX;
    [SerializeField] private AudioSource stopSFX;

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
        movingSFX.Play();
        while (transform.position.y < 5f)
        {
            transform.position += Vector3.up * openingSpeed * Time.deltaTime;
            yield return null;
        }
        movingSFX.Stop();
        pulledSFX.Play();
    }

    private IEnumerator Closing()
    {
        movingSFX.Play();
        while (transform.position.y > 0f)
        {
            transform.position -= Vector3.up * openingSpeed * Time.deltaTime;
            yield return null;
        }
        movingSFX.Stop();
        stopSFX.Play();
    }
}
