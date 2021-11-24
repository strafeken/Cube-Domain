using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrailController : MonoBehaviour
{
    private SwordAttack swordAttackEvent;
    private TrailRenderer trail;

    void Awake()
    {
        swordAttackEvent = GetComponentInParent<SwordAttack>();
        trail = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        swordAttackEvent.OnAttackStart += OnAttackStartEvent;
        swordAttackEvent.OnAttackEnd += OnAttackEndEvent;
    }

    void OnDestroy()
    {
        swordAttackEvent.OnAttackStart -= OnAttackStartEvent;
        swordAttackEvent.OnAttackEnd -= OnAttackEndEvent;
    }

    void OnAttackStartEvent()
    {
        trail.Clear();
    }

    void OnAttackEndEvent()
    {
        trail.Clear();
    }
}
