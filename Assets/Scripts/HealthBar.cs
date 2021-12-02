using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private TMP_Text text;

    [SerializeField] private Health health;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        health.OnDamaged += UpdateHealthBar;
        text.text = "HP: " + health.GetCurrentHealth() + " / " + health.GetMaxHealth();
    }

    void OnDestroy()
    {
        health.OnDamaged -= UpdateHealthBar;
    }

    private void UpdateHealthBar()
    {
        text.text = "HP: " + health.GetCurrentHealth() + " / " + health.GetMaxHealth();
    }
}
