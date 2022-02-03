using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndingManager : MonoBehaviour
{
    private Health playerHealth;

    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private float endingCanvasFadeDuration = 1f;
    private readonly float fadeSpeed = 1f;
    [SerializeField] private CanvasGroup endingCanvas;
    [SerializeField] private CanvasGroup winCanvas;
    private float timer;

    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void OnEnable()
    {
        playerHealth.OnDeath += OnPlayerDeath;
    }

    void OnDisable()
    {
        playerHealth.OnDeath -= OnPlayerDeath;
    }

    public void WinEnding()
    {
        inGameCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine("WinScreen");
    }

    private void OnPlayerDeath()
    {
        inGameCanvas.gameObject.SetActive(false);
        endingCanvas.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine("EndingScreen");
    }

    private IEnumerator WinScreen()
    {
        while (winCanvas.alpha < 1f)
        {
            timer += fadeSpeed * Time.deltaTime;
            winCanvas.alpha = timer / endingCanvasFadeDuration;
            yield return null;
        }
    }

    private IEnumerator EndingScreen()
    {
        while (endingCanvas.alpha < 1f)
        {
            timer += fadeSpeed * Time.deltaTime;
            endingCanvas.alpha = timer / endingCanvasFadeDuration;
            yield return null;
        }
    }
}
