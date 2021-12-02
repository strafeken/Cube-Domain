using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageOfFortune : MonoBehaviour
{
    [SerializeField] private GameObject[] bars;
    [SerializeField] private GameObject[] barriers;

    [SerializeField] private float cageRadius = 8;

    void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void UpdateFormation()
    {
        for (int i = 0; i < 6; ++i)
        {
            float angle = i * Mathf.PI * 2f / 6;
            Vector3 pillarPosition = new Vector3(Mathf.Cos(angle) * cageRadius, 2.5f, Mathf.Sin(angle) * cageRadius);
            bars[i].transform.position = pillarPosition;
        }

        for (int i = 0; i < 6; ++i)
        {
            Vector3 barrierPosition;
            if (i < 5)
            {
                barrierPosition = (bars[i + 1].transform.position + bars[i].transform.position) * 0.5f;
            }
            else
            {
                barrierPosition = (bars[i].transform.position + bars[0].transform.position) * 0.5f;
            }

            Quaternion barrierOrientation = Quaternion.identity;
            switch (i)
            {
                case 0:
                    barrierOrientation = Quaternion.Euler(0, 60, 0);
                    break;
                case 1:
                    barrierOrientation = Quaternion.identity;
                    break;
                case 2:
                    barrierOrientation = Quaternion.Euler(0, -60, 0);
                    break;
                case 3:
                    barrierOrientation = Quaternion.Euler(0, 60, 0);
                    break;
                case 4:
                    barrierOrientation = Quaternion.identity;
                    break;
                case 5:
                    barrierOrientation = Quaternion.Euler(0, -60, 0);
                    break;
            }
            barriers[i].transform.position = barrierPosition;
            barriers[i].transform.rotation = barrierOrientation;
        }
    }
}
