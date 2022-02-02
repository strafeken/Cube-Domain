using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFortune : MonoBehaviour
{
    private Transform player;
    [SerializeField] private WaveManager waveManager;

    [Header("Spear")]
    [SerializeField] private GameObject spear;
    [SerializeField] private float spearSpeed = 10f;
    [SerializeField] private float spearTravelTime = 1.5f;
    [SerializeField] private float emergeDistanceFromPlayer = 10f;
    [SerializeField] private float spearEmergeDelay = 0.5f;
    [SerializeField] private float timeBetweenSpears = 4f;

    [Header("Laser")]
    [SerializeField] private GameObject laser;
    [SerializeField] private float laserDuration = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    private float laserTimer = 0f;

    [Header("Missile")]
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform missilePosition;
    [SerializeField] private float missileRadius = 10f;
    [SerializeField] private float missileSpeed = 5f;
    [SerializeField] private float timeUntilShootingStarts = 2f;
    [SerializeField] private float shootingCooldown = 1f;
    [SerializeField] private float timeBetweenMissileShots = 7f;

    [Header("Cage")]
    [SerializeField] private GameObject cage;
    [SerializeField] private float cageRadius = 2f;
    [SerializeField] private float risingSpeed = 1f;
    [SerializeField] private Transform cageEndPosition;

    [Header("Ground Crack")]
    [SerializeField] private GameObject groundCrack;
    [SerializeField] private Vector3 targetCrackScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 originalCrackScale = new Vector3(0.1f, 0.5f, 0.1f);
    [SerializeField] private float scaleDuration = 1.5f;

    private bool isWaveOngoing = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void OnEnable()
    {
        waveManager.OnWaveStart += OnWaveStart;
    }

    void OnDisable()
    {
        waveManager.OnWaveStart -= OnWaveStart;
    }

    private void OnWaveStart(int waveCount)
    {
        isWaveOngoing = true;

        switch (waveCount)
        {
            case 0:
                StartCoroutine("Cage");
                //StartCoroutine("Impale");
                //StartCoroutine("Missile");
                break;
            case 1:
                //StartCoroutine("Missile");
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                StartCoroutine("Missile");
                break;
            case 5:
                StartCoroutine("Cage");
                break;
        }
    }

    private void OnWaveEnd()
    {
        isWaveOngoing = false;
    }

    private IEnumerator Impale()
    {
        while (isWaveOngoing)
        {
            if (Vector3.Distance(Vector3.zero, player.position) < 25f) // Distance from the center of the map
            {
                Vector3 crackPoint = player.position + player.forward * emergeDistanceFromPlayer;
                crackPoint.y = 0.01f;

                // Instantiate a crack
                GameObject spawnedCrack = Instantiate(groundCrack, crackPoint, Quaternion.identity);
                Transform crackT = spawnedCrack.transform;
                crackT.localScale = originalCrackScale;

                // Increase its scale over time
                float timeElapsed = 0f;
                while (timeElapsed < scaleDuration)
                {
                    crackT.localScale = Vector3.Lerp(originalCrackScale, targetCrackScale, timeElapsed / scaleDuration);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                yield return new WaitForSeconds(spearEmergeDelay);

                GameObject spawnedSpear = Instantiate(spear, crackPoint, Quaternion.identity);

                Transform spearTransform = spawnedSpear.transform;
                spearTransform.up = (player.position - spearTransform.position).normalized;

                Transform tip = spearTransform.Find("Tip").GetComponent<Transform>();

                // The value needed to move the tip to crack
                Vector3 absoluteMovement = crackPoint - tip.position;
                spearTransform.position += absoluteMovement;

                spawnedSpear.GetComponent<SpearOfFortune>().Shoot(player.position + (Vector3.up * 0.5f), spearSpeed, spearTravelTime);
            }

            yield return new WaitForSeconds(timeBetweenSpears);
        }
    }

    private IEnumerator Missile()
    {
        while (isWaveOngoing)
        {
            List<GameObject> missiles = new List<GameObject>();

            for (int i = 0; i < 5; ++i)
            {
                float angle = i * Mathf.PI * 2f / 5;
                Vector3 pos = new Vector3(transform.position.x + Mathf.Cos(angle) * missileRadius, transform.position.y, transform.position.x + Mathf.Sin(angle) * missileRadius);
                missiles.Add(Instantiate(missile, pos, Quaternion.identity));
            }

            yield return new WaitForSeconds(timeUntilShootingStarts);

            for (int i = 0; i < 5; ++i)
            {
                missiles[i].GetComponent<MissileOfFortune>().Shoot(missileSpeed);
                yield return new WaitForSeconds(shootingCooldown);
            }

            yield return new WaitForSeconds(timeBetweenMissileShots);
        }
    }

    private IEnumerator Cage()
    {
        GameObject go = Instantiate(cage, new Vector3(player.position.x, -2.5f, player.position.z), Quaternion.identity);
        Transform cageTransform = go.GetComponent<Transform>();

        while (cageTransform.position.y < cageEndPosition.position.y)
        {
            cageTransform.position += Vector3.up * risingSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(10f);
    }
}
