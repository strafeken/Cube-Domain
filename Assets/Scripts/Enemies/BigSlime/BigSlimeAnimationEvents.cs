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

    public void Move()
    {
        bigSlime.StartMove();
    }
}
