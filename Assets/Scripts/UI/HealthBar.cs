using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private TMP_Text text;
    private Slider slider;
    [SerializeField] private Health health;

    void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        health.OnDamaged += UpdateHealthBar;
    }

    void Start()
    {
        text.text = "HP: " + health.GetCurrentHealth() + " / " + health.GetMaxHealth();
        slider.value = health.GetMaxHealth();
    }

    void OnDestroy()
    {
        health.OnDamaged -= UpdateHealthBar;
    }

    private void UpdateHealthBar()
    {
        text.text = "HP: " + health.GetCurrentHealth() + " / " + health.GetMaxHealth();
        slider.value = health.GetCurrentHealth();
    }
}
