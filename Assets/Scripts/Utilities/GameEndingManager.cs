using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndingManager : MonoBehaviour
{
    private Health playerHealth;

    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void OnEnable()
    {
        playerHealth.OnDeath += OnPlayerDeath;
    }

    void OnDisable()
    {
        playerHealth.OnDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
