using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeAnimationEvents : MonoBehaviour
{
    private BigSlime bigSlime;

    void Awake()
    {
        bigSlime = GetComponentInParent<BigSlime>();
    }

    public void OnShiftStart()
    {
        bigSlime.isAtOriginalSize = false;
    }

    public void Move()
    {
        bigSlime.StartMove();
    }

    public void OnSizeReverted()
    {
        bigSlime.SizeReverted();
    }

    public void Attack()
    {
        bigSlime.StartAttack();
    }
}
