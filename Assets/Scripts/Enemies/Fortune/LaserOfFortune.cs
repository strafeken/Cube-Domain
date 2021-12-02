using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOfFortune : MonoBehaviour
{
    [SerializeField] private GameObject[] beam;
    private LineRenderer[] beamRenderer;
    private ExpandLaser[] laserBeam;
    [SerializeField] private float beamLength = 8f;
    [SerializeField] private float expandSpeed = 2f;

    // Can't seem to cache these components in Awake and Start
    public void Shoot()
    {
        for (int i = 0; i < 4; ++i)
        {
            beam[i].GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
            beam[i].GetComponent<ExpandLaser>().Expand(beamLength, expandSpeed);
        }
    }
}
