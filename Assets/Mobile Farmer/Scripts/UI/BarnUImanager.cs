using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarnUImanager : MonoSingleton<BarnUImanager>
{
    [SerializeField] GameObject barnUIContiner;
    [SerializeField] List<WorkerContiner> workerContiners = new List<WorkerContiner>();
    public Action<int> hireButtonPressed;
    public Action closeButtonPressed;



    public void ShowWorkerData(List<WorkerStat> workers)
    {
        for(int i=0 ; i<workers.Count ; i++ )
        {
            workerContiners[i].img_workerImg.sprite=workers[i].workerAvater;
            workerContiners[i].text_workerName.text = "Worker "+(i+1);
            workerContiners[i].img_workingCropImg.sprite=workers[i].workableCorp.uiIconSprite;
            if(workers[i].price > 0)
            {
                workerContiners[i].text_amount.text = CoinSystem.ConvertCoinToString(workers[i].price);
                workerContiners[i].text_button.text = (workers[i].isPurchesed) ? "Upgrade" : "Hire";

            }else{
                workerContiners[i].text_amount.text = "";
                workerContiners[i].text_button.text = "Max";
            }
        }
        barnUIContiner.SetActive(true);
    }

    public void OnHireButtonPressed(int index)
    {
        hireButtonPressed?.Invoke(index);
    }

    public void CloseButtonPressed()
    {
        closeButtonPressed?.Invoke();
        barnUIContiner.SetActive(false);
    }

   
}





