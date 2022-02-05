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
    public static WaveManager Instance { get; private set; }

    private SceneLoadManager sceneLoadManager;

    [SerializeField] private Transform[] enemyCages;
    private Transform[] spawnDirection;

    private GameObject[] gatesObject;
    private Gate[] gates;

    [Header("Enemy prefabs")]
    [SerializeField] private GameObject slime;
    [SerializeField] private GameObject bigSlime;
    [SerializeField] private GameObject shifter;
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

    [Header("Lock")]
    private Transform player;
    [SerializeField] private Transform playerStartPosition;
    [SerializeField] private float pullSpeed = 5f;
    public bool lockPlayer = true;
    [SerializeField] private GameObject[] lockIcon = new GameObject[2];

    [Header("SFX")]
    [SerializeField] private AudioSource lockSFX;

    void Awake()
    {
        Instance = this;

        sceneLoadManager = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();

        gatesObject = GameObject.FindGameObjectsWithTag("Gate");
        gates = new Gate[8];
        for (int i = 0; i < 8; ++i)
            gates[i] = gatesObject[i].GetComponent<Gate>();

        spawnDirection = new Transform[8];
        for(int i = 0; i < 8; ++i)
            spawnDirection[i] = enemyCages[i].Find("SpawnDirection").GetComponent<Transform>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        player = p.GetComponent<Transform>();
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
        lockPlayer = true;
        ToggleLockIcons();

        while (Vector3.Distance(player.position, playerStartPosition.position) > 0.2f)
        {
            player.position = Vector3.MoveTowards(player.position, playerStartPosition.position, pullSpeed * Time.deltaTime);
            yield return null;
        }

        player.position = playerStartPosition.position;

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

        lockPlayer = false;
        ToggleLockIcons();
    }

    private void SpawnEnemies()
    {
        int cageIndex = 0;

        for (int i = 0; i < waveEnemiesController[currentWave].numOfSlimes; ++i)
        {
            GameObject spawn = Instantiate(slime, new Vector3(enemyCages[cageIndex].position.x, slimeY.position.y, enemyCages[cageIndex].position.z), Quaternion.identity);
            spawn.transform.LookAt(new Vector3(0, slimeY.position.y, 0), Vector3.up);
            ++cageIndex;
        }

        for (int i = 0; i < waveEnemiesController[currentWave].numOfBigSlimes; ++i)
        {
            GameObject spawn = Instantiate(bigSlime, new Vector3(enemyCages[cageIndex].position.x, bigSlimeY.position.y, enemyCages[cageIndex].position.z), spawnDirection[cageIndex].rotation);
            spawn.transform.LookAt(new Vector3(0, bigSlimeY.position.y, 0), Vector3.up);
            ++cageIndex;
        }

        for (int i = 0; i < waveEnemiesController[currentWave].numOfShifters; ++i)
        {
            GameObject spawn = Instantiate(shifter, new Vector3(enemyCages[cageIndex].position.x, slimeY.position.y, enemyCages[cageIndex].position.z), spawnDirection[cageIndex].rotation);
            spawn.transform.LookAt(new Vector3(0, slimeY.position.y, 0), Vector3.up);
            ++cageIndex;
        }

        for (int i = 0; i < waveEnemiesController[currentWave].numOfTwirlers; ++i)
        {
            GameObject spawn = Instantiate(twirler, new Vector3(enemyCages[cageIndex].position.x, twirlerY.position.y, enemyCages[cageIndex].position.z), spawnDirection[cageIndex].rotation);
            spawn.transform.LookAt(new Vector3(0, twirlerY.position.y, 0), Vector3.up);
        }

        EnemyManager.Instance.AddEnemies();

        OnWaveStart?.Invoke(currentWave);
    }

    private void StartNewWave()
    {
        OnWaveEnd?.Invoke();
        ++currentWave;
        // Game is finished
        if(currentWave == 6)
        {
            GameEndingManager.Instance.WinEnding();
            return;
        }
        StartCoroutine(NewWave());
    }

    private IEnumerator NewWave()
    {
        lockPlayer = true;
        ToggleLockIcons();

        while (Vector3.Distance(player.position, playerStartPosition.position) > 0.2f)
        {
            player.position = Vector3.MoveTowards(player.position, playerStartPosition.position, pullSpeed * Time.deltaTime);
            yield return null;
        }

        player.position = playerStartPosition.position;

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
    
        lockPlayer = false;
        ToggleLockIcons();
    }

    private void ToggleLockIcons()
    {
        lockSFX.Play();
        for (int i = 0; i < 2; ++i)
        {
            lockIcon[i].SetActive(!lockIcon[i].activeSelf);
        }
    }
}
