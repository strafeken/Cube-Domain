using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFortune : MonoBehaviour
{
    private Transform player;
    [SerializeField] private WaveManager waveManager;

    [Header("Geyser")]
    [SerializeField] private GameObject geyser;
    [SerializeField] private float geyserSpeed = 15f;
    [SerializeField] private float gEmergeDistanceFromPlayer = 5f;
    [SerializeField] private float geyserEmergeDelay = 0.5f;
    [SerializeField] private float timeBetweenGeysers = 4f;
    [SerializeField] private Vector3 gCrackTargetScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private float gScaleDuration = 1.5f;

    [Header("Spear")]
    [SerializeField] private GameObject spear;
    [SerializeField] private float spearSpeed = 10f;
    [SerializeField] private float sEmergeDistanceFromPlayer = 10f;
    [SerializeField] private float spearEmergeDelay = 0.5f;
    [SerializeField] private float timeBetweenSpears = 4f;

    [Header("Bonfire")]
    [SerializeField] private float bonfireSpeed = 15f;
    [SerializeField] private float bEmergeDistanceFromPlayer = 5f;
    [SerializeField] private float bonfireEmergeDelay = 0.5f;
    [SerializeField] private float timeBetweenBonfires = 4f;
    [SerializeField] private Vector3 bCrackTargetScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private float bScaleDuration = 1.5f;

    [Header("Laser")]
    [SerializeField] private GameObject[] lasers = new GameObject[4];
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Missile")]
    [SerializeField] private GameObject missile;
    [SerializeField] private float missileRadius = 10f;
    [SerializeField] private float missileSpeed = 5f;
    [SerializeField] private float timeUntilShootingStarts = 2f;
    [SerializeField] private float shootingCooldown = 1f;
    [SerializeField] private float timeBetweenMissileShots = 7f;

    [Header("Cage")]
    [SerializeField] private GameObject cage;
    [SerializeField] private float risingSpeed = 1f;
    [SerializeField] private Transform cageEndPosition;

    [Header("Ground Crack")]
    [SerializeField] private GameObject groundCrack;
    [SerializeField] private Vector3 targetCrackScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 originalCrackScale = new Vector3(0.1f, 0.5f, 0.1f);
    [SerializeField] private float scaleDuration = 1.5f;

    [Header("Wave")]
    [SerializeField] private Transform body;
    private bool isWaveOngoing = false;
    private IEnumerator coroutine;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void OnEnable()
    {
        waveManager.OnWaveStart += OnWaveStart;
        waveManager.OnWaveEnd += OnWaveEnd;
    }

    void OnDisable()
    {
        waveManager.OnWaveStart -= OnWaveStart;
        waveManager.OnWaveEnd -= OnWaveEnd;
    }

    private void OnWaveStart(int waveCount)
    {
        isWaveOngoing = true;

        switch (waveCount)
        {
            case 0:
                body.rotation = Quaternion.Euler(0, 0, -90);
                coroutine = Geyser();
                break;
            case 1:
                body.rotation = Quaternion.Euler(-90, 0, 0);
                coroutine = Impale();
                break;
            case 2:
                body.rotation = Quaternion.Euler(0, 0, 180);
                coroutine = Bonfire();
                break;
            case 3:
                body.rotation = Quaternion.Euler(0, 0, 0);
                coroutine = Laser();
                break;
            case 4:
                body.rotation = Quaternion.Euler(90, 0, 0);
                coroutine = Missile();
                break;
            case 5:
                body.rotation = Quaternion.Euler(0, 0, 90);
                coroutine = Cage();
                break;
        }

        StartCoroutine(coroutine);
    }

    private void OnWaveEnd()
    {
        isWaveOngoing = false;
    }

    private IEnumerator Geyser()
    {
        yield return new WaitForSeconds(timeBetweenGeysers);

        while (isWaveOngoing)
        {
            if (Vector3.Distance(Vector3.zero, player.position) < 25f) // Distance from the center of the map
            {
                Vector3 crackPoint = player.position + player.forward * gEmergeDistanceFromPlayer;
                crackPoint.y = 0.01f;

                // Instantiate a crack
                GameObject spawnedCrack = Instantiate(groundCrack, crackPoint, Quaternion.identity);
                Transform crackT = spawnedCrack.transform;
                crackT.localScale = originalCrackScale;

                // Increase its scale over time
                float timeElapsed = 0f;
                while (timeElapsed < gScaleDuration)
                {
                    crackT.localScale = Vector3.Lerp(originalCrackScale, gCrackTargetScale, timeElapsed / gScaleDuration);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                yield return new WaitForSeconds(geyserEmergeDelay);

                GameObject spawnedGeyser = Instantiate(geyser, crackPoint, Quaternion.identity);

                Transform geyserTransform = spawnedGeyser.transform;

                Transform tip = geyserTransform.Find("Tip").GetComponent<Transform>();

                // The value needed to move the tip to crack
                Vector3 absoluteMovement = crackPoint - tip.position;
                geyserTransform.position += absoluteMovement;

                spawnedGeyser.GetComponent<GeyserOfFortune>().Shoot(geyserSpeed);
            }

            yield return new WaitForSeconds(timeBetweenGeysers);
        }
    }

    private IEnumerator Impale()
    {
        yield return new WaitForSeconds(timeBetweenSpears);

        while (isWaveOngoing)
        {
            if (Vector3.Distance(Vector3.zero, player.position) < 25f) // Distance from the center of the map
            {
                Vector3 crackPoint = player.position + player.forward * sEmergeDistanceFromPlayer;
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

                spawnedSpear.GetComponent<SpearOfFortune>().Shoot(spearSpeed);
            }

            yield return new WaitForSeconds(timeBetweenSpears);
        }
    }

    private IEnumerator Bonfire()
    {
        yield return new WaitForSeconds(timeBetweenBonfires);

        while (isWaveOngoing)
        {
            if (Vector3.Distance(Vector3.zero, player.position) < 25f) // Distance from the center of the map
            {
                Vector3[] crackPoints = new Vector3[] { player.position + player.forward * bEmergeDistanceFromPlayer, player.position - player.forward * bEmergeDistanceFromPlayer };
                crackPoints[0].y = crackPoints[1].y = 0.01f;

                // Instantiate a crack
                GameObject[] spawnedCracks = new GameObject[] { Instantiate(groundCrack, crackPoints[0], Quaternion.identity), Instantiate(groundCrack, crackPoints[1], Quaternion.identity) };
                Transform[] cracksT = new Transform[] { spawnedCracks[0].transform, spawnedCracks[1].transform };
                cracksT[0].localScale = cracksT[1].localScale = originalCrackScale;

                // Increase its scale over time
                float timeElapsed = 0f;
                while (timeElapsed < bScaleDuration)
                {
                    for (int i = 0; i < 2; ++i)
                    {
                        cracksT[i].localScale = Vector3.Lerp(originalCrackScale, bCrackTargetScale, timeElapsed / bScaleDuration);
                    }
                    timeElapsed += Time.deltaTime;

                    yield return null;
                }

                yield return new WaitForSeconds(bonfireEmergeDelay);

                GameObject[] spawnedSpears = new GameObject[] { Instantiate(spear, crackPoints[0], Quaternion.identity), Instantiate(spear, crackPoints[1], Quaternion.identity) };

                Transform[] spearTransforms = new Transform[] { spawnedSpears[0].transform, spawnedSpears[1].transform };
                spearTransforms[0].up = (player.position - spearTransforms[0].position).normalized;
                spearTransforms[1].up = (player.position - spearTransforms[1].position).normalized;

                Transform[] tips = new Transform[] { spearTransforms[0].Find("Tip").GetComponent<Transform>(), spearTransforms[1].Find("Tip").GetComponent<Transform>() };

                // The value needed to move the tip to crack
                for (int i = 0; i < 2; ++i)
                {
                    Vector3 absoluteMovement = crackPoints[i] - tips[i].position;
                    spearTransforms[i].position += absoluteMovement;

                    spawnedSpears[i].GetComponent<SpearOfFortune>().Shoot(bonfireSpeed);
                }
            }

            yield return new WaitForSeconds(timeBetweenBonfires);
        }
    }

    private IEnumerator Laser()
    {
        for (int i = 0; i < 4; ++i)
        {
            lasers[i].SetActive(true);
            lasers[i].GetComponent<LaserOfFortune>().Shoot();
        }

        while (isWaveOngoing)
        {
            for (int i = 0; i < 4; ++i)
                lasers[i].transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            yield return null;
        }

        for (int i = 0; i < 4; ++i)
            lasers[i].SetActive(false);
    }

    private IEnumerator Missile()
    {
        yield return new WaitForSeconds(timeBetweenMissileShots);

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
    }
}
