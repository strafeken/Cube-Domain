using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DashController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Properties")]
    [SerializeField] private float dashForce = 100f;
    [SerializeField] private float dashDuration = 2f;
    [SerializeField] private float dashCooldown = 8f;
    private float dashCooldownTimer = 0f;

    private bool canDash = true;

    [Header("Others")]
    [SerializeField] private ParticleSystem dashParticleSystem;
    [SerializeField] private SwordController sword;
    private int playerLayer;
    private int enemyLayer;
    private int heartLayer;

    [Header("UI")]
    [SerializeField] private GameObject icon;
    [SerializeField] private Image iconImage;
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color inCooldownColor = Color.white;
    private Slider cooldownSlider;
    private TMP_Text cooldownText;

    [Header("SFX")]
    [SerializeField] private AudioSource dashSFX;
    [SerializeField] private AudioSource skillRefreshSFX;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        cooldownSlider = icon.GetComponent<Slider>();
        cooldownText = icon.GetComponentInChildren<TMP_Text>();

        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        heartLayer = LayerMask.NameToLayer("Heart");
    }

    void Start()
    {
        icon.SetActive(false);
        cooldownSlider.maxValue = dashCooldown;

    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (WaveManager.Instance.lockPlayer)
            return;

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
        // Invincibility
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        Physics.IgnoreLayerCollision(playerLayer, heartLayer, true);

        rb.AddForce(Camera.main.transform.forward * dashForce, ForceMode.Impulse);

        dashSFX.Play();

        canDash = false;

        yield return new WaitForSeconds(dashDuration);

        // Reset properties
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        Physics.IgnoreLayerCollision(playerLayer, heartLayer, false);

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

        skillRefreshSFX.Play();

        // Reset properties
        icon.SetActive(false);
        iconImage.color = defaultColor;
        dashCooldownTimer = 0f;
        canDash = true;
    }
}
