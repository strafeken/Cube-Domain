using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShifterAnimationEvents : MonoBehaviour
{
    private Shifter shifter;

    void Awake()
    {
        shifter = GetComponentInParent<Shifter>();
    }

    public void Disappear()
    {
        shifter.Disappear();
    }
}
