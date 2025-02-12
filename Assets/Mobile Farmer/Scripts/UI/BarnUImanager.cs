using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarnUImanager : MonoSingleton<BarnUImanager>
{
    [SerializeField] GameObject barnUIContiner;
    [SerializeField] List<WorkerContiner> workerContiners = new List<WorkerContiner>();
    public Action<int> hireButtonPressed;



    public void ShowWorkerData(List<workerStat> workers)
    {
        for(int i=0 ; i<workers.Count ; i++ )
        {
            workerContiners[i].workerImg.sprite=workers[i].workerAvater;
            workerContiners[i].workerName.text = "Worker "+(i+1);
            workerContiners[i].workingCropImg.sprite=workers[i].workableCorp.uiIconSprite;
        }
        barnUIContiner.SetActive(true);
    }

    public void OnHireButtonPressed(int index)
    {
        hireButtonPressed?.Invoke(index);
    }

   
}





[System.Serializable]
public struct WorkerContiner
{
    public Image workerImg;
    public TMP_Text workerName;
    public TMP_Text workerDescription;
    public Image workingCropImg;
}