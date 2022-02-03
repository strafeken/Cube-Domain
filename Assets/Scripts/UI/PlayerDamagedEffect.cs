using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamagedEffect : MonoBehaviour
{
    private Health player;
    private Image image;
    [SerializeField] private float showTime = 0.2f;

    private bool isCoroutineRunning = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        image = GetComponent<Image>();
    }

    void OnEnable()
    {
        player.OnDamaged += OnPlayerDamaged;
    }

    void OnDisable()
    {
        player.OnDamaged -= OnPlayerDamaged;
    }

    private void OnPlayerDamaged()
    {
        if (isCoroutineRunning)
        {
            image.enabled = false;
            StopCoroutine("ShowEffect");
        }

        StartCoroutine("ShowEffect");
    }

    private IEnumerator ShowEffect()
    {
        isCoroutineRunning = true;

        image.enabled = true;

        yield return new WaitForSeconds(showTime);

        image.enabled = false;

        isCoroutineRunning = false;
    }
}
