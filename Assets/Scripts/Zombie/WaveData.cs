using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WaveData", menuName = "WaveData", order = 1)]
public class WaveData : ScriptableObject
{
    public int fullcountOfWave;
    public int countOnField;
    public float Xhealth;
    public float Xdamage;
    public Vector2 timeBetweenSpawn;
    public float timeofFirstAgry;
    public bool isHasBoss;
    public int countofBoos;
}
