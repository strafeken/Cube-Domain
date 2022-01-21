using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DashController : MonoBehaviour
{
    private CapsuleCollider bodyCollider;
    private Rigidbody rb;

    [Header("Properties")]
    [SerializeField] private float dashForce = 100f;
    [SerializeField] private float dashDuration = 2f;
    [SerializeField] private float dashCooldown = 8f;
    private float dashCooldownTimer = 0f;

    private bool canDash = true;

    [Header("Others")]
    [SerializeField] private ParticleSystem dashParticleSystem;
    [SerializeField] private SwordAnimation sword;

    [Header("UI")]
    [SerializeField] private GameObject icon;
    [SerializeField] private Image iconImage;
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color inCooldownColor = Color.white;
    private Slider cooldownSlider;
    private TMP_Text cooldownText;

    void Awake()
    {
        bodyCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        cooldownSlider = icon.GetComponent<Slider>();
        cooldownText = icon.GetComponentInChildren<TMP_Text>();
    }

    void Start()
    {
        icon.SetActive(false);
        cooldownSlider.maxValue = dashCooldown;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (canDash)
            sword.Dash();
    }

    public void StartDash()
    {
        StartCoroutine("DashAttack");
        StartCoroutine("DashCooldown");
    }

    private IEnumerator DashAttack()
    {
        bodyCollider.isTrigger = true; // Invincibility
        rb.AddForce(Camera.main.transform.forward * dashForce, ForceMode.Impulse);

        canDash = false;

        yield return new WaitForSeconds(dashDuration);

        // Reset properties
        bodyCollider.isTrigger = false;
        rb.velocity = Vector3.zero;
    }

    private IEnumerator DashCooldown()
    {
        icon.SetActive(true);
        iconImage.color = inCooldownColor;
        dashParticleSystem.Play();

        while (dashCooldownTimer < dashCooldown)
        {
            dashCooldownTimer += Time.deltaTime;
            cooldownSlider.value = dashCooldownTimer;
            cooldownText.text = Mathf.Ceil(cooldownSlider.value).ToString();
            yield return null;
        }

        // Reset properties
        icon.SetActive(false);
        iconImage.color = defaultColor;
        dashCooldownTimer = 0f;
        canDash = true;
    }
}
