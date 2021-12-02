using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Must run before SceneLoadManager so that it can subscribe before it invokes the event
/// </summary>
public class WaveManager : MonoBehaviour
{
    private SceneLoadManager sceneLoadManager;

    [SerializeField] private Transform[] enemyCages;

    private GameObject[] gatesObject;
    private Gate[] gates;

    [Header("Enemy prefabs")]
    [SerializeField] private GameObject slime;
    [SerializeField] private GameObject bigSlime;
    [SerializeField] private GameObject twirler;
    [SerializeField] private GameObject fortune;

    [Header("Enemy Y positions")]
    [SerializeField] private Transform slimeY;
    [SerializeField] private Transform bigSlimeY;
    [SerializeField] private Transform twirlerY;
    [SerializeField] private Transform fortuneY;

    [Header("Wave Settings")]
    [SerializeField] private float timeTillFirstWaveStarts = 5f;
    [SerializeField] private float gatesClosingTime = 5f;
    [SerializeField] private float waveBufferTime = 3f;

    [SerializeField] private List<WaveEnemiesController> waveEnemiesController;

    private int currentWave = 0;

    void Awake()
    {
        sceneLoadManager = GameObject.FindGameObjectWithTag("SceneLoadManager").GetComponent<SceneLoadManager>();

        gatesObject = GameObject.FindGameObjectsWithTag("Gate");

        gates = new Gate[8];
        for (int i = 0; i < 8; ++i)
            gates[i] = gatesObject[i].GetComponent<Gate>();
    }

    void Start()
    {
        sceneLoadManager.OnSceneFinishedLoading += OnSceneLoaded;
        EnemyManager.Instance.OnAllEnemiesDead += StartNewWave;
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
        yield return new WaitForSeconds(timeTillFirstWaveStarts);

        for (int i = 0; i < 8; ++i)
        {
            gates[i].OpenGate();
        }

        SpawnEnemies();

        yield return new WaitForSeconds(gatesClosingTime);

        for (int i = 0; i < 8; ++i)
        {
            gates[i].CloseGate();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < waveEnemiesController[currentWave].numOfSlimes; ++i)
        {
            int cageIndex = Random.Range(0, 7);
            Instantiate(slime, new Vector3(enemyCages[cageIndex].position.x, slimeY.position.y, enemyCages[cageIndex].position.z), Quaternion.identity);
        }

        for (int i = 0; i < waveEnemiesController[currentWave].numOfBigSlimes; ++i)
            Instantiate(bigSlime, new Vector3(enemyCages[i].position.x, bigSlimeY.position.y, enemyCages[i].position.z), Quaternion.identity);

        for (int i = 0; i < waveEnemiesController[currentWave].numOfTwirlers; ++i)
            Instantiate(twirler, new Vector3(enemyCages[i].position.x, twirlerY.position.y, enemyCages[i].position.z), Quaternion.identity);

        for (int i = 0; i < waveEnemiesController[currentWave].numOfFortunes; ++i)
            Instantiate(fortune, new Vector3(enemyCages[i].position.x, fortuneY.position.y, enemyCages[i].position.z), Quaternion.identity);

        EnemyManager.Instance.AddEnemies();
    }

    private void StartNewWave()
    {
        ++currentWave;
        StartCoroutine(NewWave());
    }

    private IEnumerator NewWave()
    {
        yield return new WaitForSeconds(waveBufferTime);

        for (int i = 0; i < 8; ++i)
        {
            gates[i].OpenGate();
        }

        SpawnEnemies();

        yield return new WaitForSeconds(gatesClosingTime);

        for (int i = 0; i < 8; ++i)
        {
            gates[i].CloseGate();
        }
    }
}
