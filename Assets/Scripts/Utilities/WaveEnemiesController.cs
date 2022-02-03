using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WaveEnemiesController
{
    public int waveNumber;
    [Range(0, 8)]
    public int numOfSlimes;
    [Range(0, 8)]
    public int numOfBigSlimes;
    [Range(0, 8)]
    public int numOfShifters;
    [Range(0, 8)]
    public int numOfTwirlers;
}
