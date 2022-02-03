using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    public int numOfEnemies;

    public event Action OnEnemyDeath;
    public event Action OnAllEnemiesDead;

    private bool endGame = false;

    void Start()
    {
        Instance = this;
    }

    public void AddEnemies()
    {
        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        numOfEnemies = enemies.Count;
    }

    public void RemoveFromList(GameObject go)
    {
        if (endGame)
            return;

        enemies.Remove(go);
        --numOfEnemies;

        OnEnemyDeath?.Invoke();

        if(numOfEnemies < 1)
            OnAllEnemiesDead?.Invoke();
    }

    public void EndGame()
    {
        endGame = true;

        for(int i = 0; i < enemies.Count; ++i)
        {
            Destroy(enemies[i]);
        }

        enemies.Clear();
    }
}
