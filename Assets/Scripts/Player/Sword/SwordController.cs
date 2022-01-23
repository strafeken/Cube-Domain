using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwordController : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private DashController dashController;

    [Header("Properties")]
    [SerializeField] private int maxSlashCharges = 8;
    private float currentSlashCharges;
    [SerializeField] private float slashRegenRate = 3f;
    private float slashRegenTimer = 0f;

    private bool isAnimationPlaying = false;
    private bool doRightSlash = true;
    private bool doLeftSlash = true;

    const string RIGHT_SLASH = "RightSlash";
    const string LEFT_SLASH = "LeftSlash";
    const string MIDDLE_SLASH = "MiddleSlash";

    const string DASH = "Dash";
    const string DASH_SHEATHE = "DashSheathe";

    [SerializeField] private float dashSheatheBufferTime = 1f;

    private bool isRegenCoroutineRunning;

    [Header("UI")]
    [SerializeField] private GameObject icon;
    [SerializeField] private Image iconImage;
    //[SerializeField] private Color defaultColor = Color.black;
    //[SerializeField] private Color inCooldownColor = Color.white;
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
            doLeftSlash = true;
        }
        else if (doLeftSlash)
        {
            PlayAnimation(LEFT_SLASH);
            doLeftSlash = false;
        }
        else
        {
            PlayAnimation(MIDDLE_SLASH);
            doRightSlash = true;
            doLeftSlash = false;
        }
    }

    public void OnEClicked()
    {
        if (isAnimationPlaying)
            return;
    }

    public void Dash()
    {
        if (isAnimationPlaying)
            return;

        animator.Play(DASH, -1, 0f);
    }

    private void PlayAnimation(string slash)
    {
        animator.Play(slash);
        --currentSlashCharges;
        cooldownText.text = currentSlashCharges.ToString();

        if (!isRegenCoroutineRunning)
        {
            StartCoroutine("RegenCharges");
        }
    }

    private IEnumerator RegenCharges()
    {
        isRegenCoroutineRunning = true;

        icon.SetActive(true);
        //iconImage.color = inCooldownColor;

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
        //iconImage.color = defaultColor;

        isRegenCoroutineRunning = false;
    }

    private void OnRightSlashStart()
    {
        isAnimationPlaying = true;
        StopCoroutine("DashSheatheBuffer");
    }

    private void OnRightSlashFinished()
    {
        isAnimationPlaying = false;
    }

    private void OnLeftSlashStart()
    {
        isAnimationPlaying = true;
        StopCoroutine("DashSheatheBuffer");
    }

    private void OnLeftSlashFinished()
    {
        isAnimationPlaying = false;
    }

    private void OnMiddleSlashStart()
    {
        isAnimationPlaying = true;
        StopCoroutine("DashSheatheBuffer");
    }

    private void OnMiddleSlashFinished()
    {
        isAnimationPlaying = false;
    }

    private void OnDashStart()
    {
        isAnimationPlaying = true;
    }

    private void OnDashAttack()
    {
        dashController.StartDash();
    }

    private void OnDashFinished()
    {
        isAnimationPlaying = false;
        StartCoroutine("DashSheatheBuffer");
    }

    private IEnumerator DashSheatheBuffer()
    {
        yield return new WaitForSeconds(dashSheatheBufferTime);
        animator.Play(DASH_SHEATHE);
    }

    private void OnDashSheatheStart()
    {
        isAnimationPlaying = true;
    }

    private void OnDashSheatheFinished()
    {
        isAnimationPlaying = false;
        animator.Play("Idle");
    }
}
