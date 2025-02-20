using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="WorkerStat" ,menuName ="GAME/Worker Stat")]
public class WorkerStat : ScriptableObject
{
    public bool isPurchased = false;
    public Sprite workerAvater;
    public Worker workerPrefab;
    public Worker currentWorkerInstance;
    [Header("Settings")]
    [Range(1,10)]
    public int level = 1;
    [Range(0.1f,3f)]
    public float performActionDelay =2f;
    [Range(2,10)]
    public float walkSpeed = 3f;
    [Range(0.1f,3f)]
    public float moveSpeedWhileWorking = 2f;
    public int maxLoadCapacity = 20;
    public int price = 1000;

    public CropData workableCorp;

    public void Upgrade()
    {
        if(level >= 10) return;
        level++;
        price += price / 2;
        maxLoadCapacity +=  maxLoadCapacity / 2;
        if(level >= 10)
        {
            price = 0;
        }
    }
}
