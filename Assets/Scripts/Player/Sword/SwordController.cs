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

    [Header("UI")]
    [SerializeField] private GameObject icon;
    [SerializeField] private Image iconImage;
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color inCooldownColor = Color.white;
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private TMP_Text cooldownText;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentSlashCharges = maxSlashCharges;
    }

    public void OnLMBClicked()
    {
        if (isAnimationPlaying)
            return;

        if (currentSlashCharges < 1)
            return;

        if(doRightSlash)
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
