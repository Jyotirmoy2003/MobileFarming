using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="workerStat" ,menuName ="GAME/Worker Stat")]
public class workerStat : ScriptableObject
{
    public Barn allocatedBarn;
    [Header("Settings")]
    [Range(2,10)]
    public float walkSpeed = 3f;
    public int maxLoadCapacity = 20;

    public CropData workableCorp;
}
