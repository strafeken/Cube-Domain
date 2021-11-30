using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimation : MonoBehaviour
{
    public enum SwordState
    {
        IDLE,
        HORIZONTAL_SLASH,
        DIAGONAL_SLASH,
        STAB,
        SHEATHE
    }

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private SwordState state;

    [SerializeField] private float sheatheTimerCooldown = 2f;
    private bool sheathed = true;

    void Awake()
    {

    }

    void Start()
    {
        SetState(SwordState.IDLE);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(state == SwordState.IDLE && sheathed)
            {
                SetState(SwordState.HORIZONTAL_SLASH);
            }

            switch(state)
            {
                case SwordState.HORIZONTAL_SLASH:
                    animator.SetInteger("SwordState", (int)SwordState.HORIZONTAL_SLASH);
                    break;
                case SwordState.DIAGONAL_SLASH:
                    StopCoroutine("SheatheTimer");
                    animator.SetInteger("SwordState", (int)SwordState.DIAGONAL_SLASH);
                    break;
                case SwordState.STAB:
                    break;
            }
        }
    }

    private void SetState(SwordState nextState)
    {
        state = nextState;
        switch(state)
        {
            case SwordState.IDLE:
                animator.SetInteger("SwordState", (int)SwordState.IDLE);
                break;
            case SwordState.HORIZONTAL_SLASH:
                sheathed = false;
                break;
            case SwordState.DIAGONAL_SLASH:
                break;
            case SwordState.STAB:
                break;
            case SwordState.SHEATHE:
                animator.SetInteger("SwordState", (int)SwordState.SHEATHE);
                break;
        }
    }

    private void OnHorizontalSlashFinished()
    {
        SetState(SwordState.DIAGONAL_SLASH);
        StartCoroutine("SheatheTimer");
    }

    private void OnDiagonalSlashFinished()
    {
        SheatheSword();
    }
    private void OnSheatheFinished()
    {
        SetState(SwordState.IDLE);
    }
    private void OnIdle()
    {
        sheathed = true;
    }

    private IEnumerator SheatheTimer()
    {
        yield return new WaitForSeconds(sheatheTimerCooldown);
        SheatheSword();
    }

    private void SheatheSword()
    {
        SetState(SwordState.SHEATHE);
    }
}
