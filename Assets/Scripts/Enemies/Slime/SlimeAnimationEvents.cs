using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimationEvents : MonoBehaviour
{
    private Slime slime;

    void Awake()
    {
        slime = GetComponentInParent<Slime>();
    }

    public void Jump()
    {
        slime.StartJump();
    }
}
