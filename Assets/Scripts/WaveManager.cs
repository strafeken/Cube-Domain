using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private GameObject[] gates = new GameObject[8];

    void Awake()
    {
        gates = GameObject.FindGameObjectsWithTag("Gate");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
