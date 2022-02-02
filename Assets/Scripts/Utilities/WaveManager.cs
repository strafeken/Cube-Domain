using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

/// <summary>
/// Must run before SceneLoadManager so that it can subscribe before it invokes the event
/// </summary>
public class WaveManager : MonoBehaviour
{
    private SceneLoadManager sceneLoadManager;

    [SerializeField] private Transform[] enemyCages;
    private Transform[] spawnDirection;

    private GameObject[] gatesObject;
    private Gate[] gates;

    [Header("Enemy prefabs")]
    [SerializeField] private GameObject slime;
    [SerializeField] private GameObject bigSlime;
    [SerializeField] private GameObject twirler;

    [Header("Enemy Y positions")]
    [SerializeField] private Transform slimeY;
    [SerializeField] private Transform bigSlimeY;
    [SerializeField] private Transform twirlerY;

    [Header("Wave Settings")]
    [SerializeField] private float timeTillFirstWaveStarts = 5f;
    [SerializeField] private float gatesClosingTime = 5f;
    [SerializeField] private float waveBufferTime = 3f;

    [SerializeField] private List<WaveEnemiesController> waveEnemiesController;

    [SerializeField] private TMP_Text waveText;
    private float waveTimer;

    private int currentWave = 0;

    public Action<int> OnWaveStart;
    public Action OnWaveEnd;

    void Awake()
    {
        sceneLoadManager = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();

        gatesObject = GameObject.FindGameObjectsWithTag("Gate");
        gates = new Gate[8];
        for (int i = 0; i < 8; ++i)
            gates[i] = gatesObject[i].GetComponent<Gate>();

        spawnDirection = new Transform[8];
        for(int i = 0; i < 8; ++i)
            spawnDirection[i] = enemyCages[i].Find("SpawnDirection").GetComponent<Transform>();
    }

    void Start()
    {
        sceneLoadManager.OnSceneFinishedLoading += OnSceneLoaded;
        EnemyManager.Instance.OnAllEnemiesDead += StartNewWave;
        waveTimer = timeTillFirstWaveStarts;
    }

    void OnDestroy()
    {
        sceneLoadManager.OnSceneFinishedLoading -= OnSceneLoaded;
        EnemyManager.Instance.OnAllEnemiesDead -= StartNewWave;
    }

    private void OnSceneLoaded()
    {
        StartCoroutine(SceneCountdown());
    }

    private IEnumerator SceneCountdown()
    {
        while(waveTimer > 0f)
        {
            waveTimer -= Time.deltaTime;
            waveText.text = Mathf.Round(waveTimer % 60) + "s";
            yield return null;
        }

        waveText.gameObject.SetActive(false);

        for (int i = 0; i < 8; ++i)
            gates[i].OpenGate();

        SpawnEnemies();

        yield return new WaitForSeconds(gatesClosingTime);

        for (int i = 0; i < 8; ++i)
            gates[i].CloseGate();
    }

    private void SpawnEnemies()
    {
        int cageIndex;
        int[] previousSpawnIndex = new int[waveEnemiesController[currentWave].numOfSlimes];
        for (int i = 0; i < waveEnemiesController[currentWave].numOfSlimes; ++i)
        {
            // Spawn Slime at a new cage
            do
            {
                cageIndex = UnityEngine.Random.Range(0, 7);
            } while (previousSpawnIndex.Contains(cageIndex));

            Instantiate(slime, new Vector3(enemyCages[cageIndex].position.x, slimeY.position.y, enemyCages[cageIndex].position.z), spawnDirection[cageIndex].rotation);
            previousSpawnIndex[i] = cageIndex;
        }

        for (int i = 0; i < waveEnemiesController[currentWave].numOfBigSlimes; ++i)
            Instantiate(bigSlime, new Vector3(enemyCages[i].position.x, bigSlimeY.position.y, enemyCages[i].position.z), spawnDirection[i].rotation);

        for (int i = 0; i < waveEnemiesController[currentWave].numOfTwirlers; ++i)
            Instantiate(twirler, new Vector3(enemyCages[i].position.x, twirlerY.position.y, enemyCages[i].position.z), spawnDirection[i].rotation);

        EnemyManager.Instance.AddEnemies();

        OnWaveStart?.Invoke(currentWave);
    }

    private void StartNewWave()
    {
        OnWaveEnd?.Invoke();
        ++currentWave;
        StartCoroutine(NewWave());
    }

    private IEnumerator NewWave()
    {
        waveTimer = waveBufferTime;
        waveText.gameObject.SetActive(true);

        while (waveTimer > 0f)
        {
            waveTimer -= Time.deltaTime;
            waveText.text = Mathf.Round(waveTimer % 60) + "s";
            yield return null;
        }

        waveText.gameObject.SetActive(false);
        
        for (int i = 0; i < 8; ++i)
            gates[i].OpenGate();

        SpawnEnemies();

        yield return new WaitForSeconds(gatesClosingTime);

        for (int i = 0; i < 8; ++i)
            gates[i].CloseGate();
    }
}
