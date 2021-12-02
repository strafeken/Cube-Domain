using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRigidbodyController : MonoBehaviour
{
    private Twirler twirler;
    private Rigidbody rb;

    void Awake()
    {
        twirler = GetComponentInParent<Twirler>();
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        twirler.OnChangeToIdleState += OnIdleState;
        twirler.OnChangeToChaseState += OnChaseState;
        twirler.OnChangeToFireballState += OnFireballState;
        twirler.OnChangeToMultipleFireballState += OnMultipleFireballState;
        twirler.OnChangeToSpinState += OnSpinState;
    }

    void Start()
    {
        rb.freezeRotation = true;
    }

    void OnDisable()
    {
        twirler.OnChangeToIdleState -= OnIdleState;
        twirler.OnChangeToChaseState -= OnChaseState;
        twirler.OnChangeToFireballState -= OnFireballState;
        twirler.OnChangeToMultipleFireballState -= OnMultipleFireballState;
        twirler.OnChangeToSpinState -= OnSpinState;
    }

    private void OnIdleState()
    {
        rb.isKinematic = true;
    }

    private void OnChaseState()
    {
        rb.isKinematic = true;
    }

    private void OnFireballState()
    {
        rb.isKinematic = true;
    }

    private void OnMultipleFireballState()
    {
        rb.isKinematic = true;
    }

    private void OnSpinState()
    {
        rb.isKinematic = true;
    }
}
