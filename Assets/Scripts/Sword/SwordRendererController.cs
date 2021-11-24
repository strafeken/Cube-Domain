using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRendererController : MonoBehaviour
{
    private Material dissolveMaterial;
    [SerializeField] float dissolveSpeed = 1f;
    float alphaValue = 1f;

    public bool swordHasFormed;

    //private SwordAttack swordAttackEvent;
    private Renderer swordRenderer;

    void Awake()
    {
        //swordAttackEvent = GameObject.FindGameObjectWithTag("Sword").GetComponent<SwordAttack>();
        swordRenderer = GetComponent<Renderer>();
        dissolveMaterial = swordRenderer.sharedMaterial;
        //dissolveMaterial = swordRenderer.materials[0];
    }

    void Start()
    {
        swordRenderer.enabled = false;
        //swordAttackEvent.OnAttackStart += OnAttackStartEvent;
    }

    void OnDestroy()
    {
        //swordAttackEvent.OnAttackStart -= OnAttackStartEvent;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (!swordHasFormed)
            {
                StartCoroutine(CondenseSword());
            }
        }
    }

    IEnumerator CondenseSword()
    {
        swordRenderer.enabled = true;
        while (alphaValue > 0f)
        {
            alphaValue -= dissolveSpeed * Time.deltaTime;
            dissolveMaterial.SetFloat("Alpha_Ref", alphaValue);
            yield return null;
        }
        swordHasFormed = true;
    }

    void OnAttackStartEvent()
    {
        ResetValues();
    }

    void ResetValues()
    {
        swordRenderer.enabled = false;
        swordHasFormed = false;
        alphaValue = 1f;
    }
}
