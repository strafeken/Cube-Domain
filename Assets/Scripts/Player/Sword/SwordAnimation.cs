using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimation : MonoBehaviour
{
    public enum SwordMode
    {
        SLASH,
        STAB
    }

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private SwordMode mode;

    [SerializeField] private float sheatheTimerCooldown = 2f;

    const string IDLE = "Idle";
    const string HORIZONTAL_SLASH = "HorizontalSlash";
    const string DIAGONAL_SLASH = "DiagonalSlash";
    const string VERTICAL_SLASH = "VerticalSlash";
    const string SHEATHE = "Sheathe";
    const string LEFT_SLASH = "LeftSlash";

    const string STAB = "Stab";

    private bool isAnimationPlaying = false;

    private string currentState;
    private string stateBeforeStab;

    [SerializeField] private DashController dashController;

    void Start()
    {
        mode = SwordMode.SLASH;

        currentState = IDLE;
    }

    public void OnLMBClicked()
    {
        if (isAnimationPlaying)
            return;

        if (mode == SwordMode.SLASH)
        {
            animator.Play(LEFT_SLASH, -1, 0f);
            //switch (currentState)
            //{
            //    case IDLE:
            //        PlayAnimation(HORIZONTAL_SLASH);
            //        break;
            //    case HORIZONTAL_SLASH:
            //        StopCoroutine("SheatheTimer");
            //        PlayAnimation(DIAGONAL_SLASH);
            //        break;
            //    case DIAGONAL_SLASH:
            //        StopCoroutine("SheatheTimer");
            //        PlayAnimation(VERTICAL_SLASH);
            //        break;
            //    case LEFT_SLASH:
            //        animator.Play(LEFT_SLASH, -1, 0f);
            //        //PlayAnimation(LEFT_SLASH);
            //        break;
            //}
        }
        else
        {
            StopCoroutine("SheatheTimer");
            PlayAnimation(STAB);
        }
    }

    private void PlayAnimation(string newState)
    {
        if(mode == SwordMode.SLASH)
        {
            if (string.Compare(currentState, newState) == 0)
                return;

            animator.Play(newState);
        }
        else
        {
            animator.Play(STAB, -1, 0f);
        }

        currentState = newState;

        isAnimationPlaying = true;
    }

    private void OnHorizontalSlashFinished()
    {
        isAnimationPlaying = false;
        StartCoroutine("SheatheTimer");
    }

    private void OnDiagonalSlashFinished()
    {
        isAnimationPlaying = false;
        StartCoroutine("SheatheTimer");
    }

    private void OnVerticalSlashFinished()
    {
        isAnimationPlaying = false;
        SheatheSword();
    }

    private IEnumerator SheatheTimer()
    {
        yield return new WaitForSeconds(sheatheTimerCooldown);
        SheatheSword();
    }

    private void SheatheSword()
    {
        mode = SwordMode.SLASH;
        PlayAnimation(SHEATHE);
    }

    private void OnSheatheFinished()
    {
        isAnimationPlaying = false;
        PlayAnimation(IDLE);
    }

    private void OnIdle()
    {
        isAnimationPlaying = false;
    }

    private void OnStabFinished()
    {
        isAnimationPlaying = false;
        StartCoroutine("SheatheTimer");
    }

    private void OnDashStart()
    {
        StopCoroutine("SheatheTimer");
    }

    private void OnDashAttack()
    {
        dashController.StartDash();
    }

    private void OnDashFinished()
    {
        isAnimationPlaying = false;
        StartCoroutine("SheatheTimer");
    }

    public void ReceiveInput(float value)
    {
        if (currentState == IDLE || currentState == SHEATHE)
            return;

        if (value > 0)
        {
            stateBeforeStab = currentState; // Save state before stab
            mode = SwordMode.STAB;
        }
        else if (value < 0)
        {
            if (mode == SwordMode.SLASH)
                return;

            mode = SwordMode.SLASH;

            currentState = stateBeforeStab;
        }
    }

    public void Dash()
    {
        if(!isAnimationPlaying)
        {
            animator.Play("Dash", -1, 0f);
            isAnimationPlaying = true;
        }
    }
}
