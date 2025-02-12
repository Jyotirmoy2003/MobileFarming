using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="workerStat" ,menuName ="GAME/Worker Stat")]
public class workerStat : ScriptableObject
{
    public Sprite workerAvater;
    public Worker workerPrefab;
    public Barn allocatedBarn;
    [Header("Settings")]
    [Range(0.1f,3f)]
    public float performActionDelay =2f;
    [Range(2,10)]
    public float walkSpeed = 3f;
    [Range(0.1f,3f)]
    public float moveSpeedWhileWorking = 2f;
    public int maxLoadCapacity = 20;

    public CropData workableCorp;
}
