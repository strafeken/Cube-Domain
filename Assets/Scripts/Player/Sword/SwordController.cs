using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwordController : MonoBehaviour
{
    private Animator animator;

    [Header("Properties")]
    [SerializeField] private int maxSlashCharges = 8;
    private float currentSlashCharges;
    [SerializeField] private float slashRegenRate = 3f;
    private float slashRegenTimer = 0f;

    private bool isAnimationPlaying = false;
    private bool doRightSlash = true;

    const string RIGHT_SLASH = "RightSlash";
    const string LEFT_SLASH = "LeftSlash";

    private IEnumerator regenCoroutine;
    private bool isRegenCoroutineRunning;

    [Header("UI")]
    [SerializeField] private GameObject icon;
    [SerializeField] private Image iconImage;
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color inCooldownColor = Color.white;
    private Slider cooldownSlider;
    [SerializeField] private TMP_Text cooldownText;

    void Awake()
    {
        animator = GetComponent<Animator>();

        cooldownSlider = icon.GetComponent<Slider>();
    }

    void Start()
    {
        icon.SetActive(false);

        currentSlashCharges = maxSlashCharges;
        cooldownText.text = currentSlashCharges.ToString();

        regenCoroutine = RegenCharges();
    }

    public void OnLMBClicked()
    {
        if (isAnimationPlaying)
            return;

        if (currentSlashCharges < 1)
            return;

        if (doRightSlash)
        {
            PlayAnimation(RIGHT_SLASH);
            doRightSlash = false;
        }
        else
        {
            PlayAnimation(LEFT_SLASH);
            doRightSlash = true;
        }
    }

    private void PlayAnimation(string slash)
    {
        animator.Play(slash);
        --currentSlashCharges;
        Debug.Log("PlayAnimation");
        if (!isRegenCoroutineRunning)
        {
            Debug.Log("StartCoroutine");
            StartCoroutine(regenCoroutine);
        }
    }

    private IEnumerator RegenCharges()
    {
        Debug.Log("Running");
        isRegenCoroutineRunning = true;

        icon.SetActive(true);
        iconImage.color = inCooldownColor;

        while (currentSlashCharges < maxSlashCharges)
        {
            slashRegenTimer += Time.deltaTime;
            cooldownSlider.value = slashRegenTimer;

            if (slashRegenTimer > slashRegenRate)
            {
                ++currentSlashCharges;
                slashRegenTimer = 0f;
            }

            cooldownText.text = currentSlashCharges.ToString();

            yield return null;
        }

        icon.SetActive(false);
        iconImage.color = defaultColor;

        isRegenCoroutineRunning = false;
        Debug.Log("Finished");
    }

    private void OnRightSlashStart()
    {
        isAnimationPlaying = true;
    }

    private void OnRightSlashFinished()
    {
        isAnimationPlaying = false;
    }

    private void OnLeftSlashStart()
    {
        isAnimationPlaying = true;
    }

    private void OnLeftSlashFinished()
    {
        isAnimationPlaying = false;
    }
}
