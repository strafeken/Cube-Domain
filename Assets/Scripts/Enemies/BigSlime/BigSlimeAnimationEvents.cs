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
        bigSlime.isReadyToMove = false;
    }

    public void Move()
    {
        bigSlime.StartMove();
    }
    public void Attack()
    {
        bigSlime.StartAttack();
    }

    public void OnSizeReverted()
    {
        bigSlime.SizeReverted();
    }
}
