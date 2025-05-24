using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarnUImanager : MonoSingleton<BarnUImanager>
{
    [SerializeField] GameObject barnUIContiner;
    [SerializeField] List<WorkerContiner> workerContiners = new List<WorkerContiner>();
    [SerializeField] Button barnUpgradbutton;
    [SerializeField] TMP_Text barnUpgradeText;
    [SerializeField] TMP_Text barnCapacityText;
    public Action<int> hireButtonPressed,clothButtonPressed;
    public Action closeButtonPressed;



    public void ShowWorkerData(List<WorkerStat> workers,List<CropField> nearbyFields)
    {
        for(int i=0 ; i<workers.Count ; i++ )
        {
            workerContiners[i].clothButton.interactable = workers[i].isPurchased;
            workerContiners[i].img_workerImg.sprite=workers[i].workerAvater;
            workerContiners[i].text_workerName.text = "Worker "+(i+1);
            workerContiners[i].img_workingCropImg.sprite=workers[i].workableCorp.uiIconSprite;
            workerContiners[i].upgradeSlider.value = workers[i].level;
            if (workers[i].price > 0)
            {
                workerContiners[i].text_amount.text = CoinSystem.ConvertCoinToString(workers[i].price);
                workerContiners[i].text_button.text = (workers[i].isPurchased) ? "Upgrade" : "Hire";

                workerContiners[i].hireButton.interactable = false;
                foreach (CropField item in nearbyFields)
                {
                    if (item.GetCropData() == workers[i].workableCorp && item.cropFieldDataHolder.chunk.IsUnclocked())
                    {
                        workerContiners[i].hireButton.interactable = true;
                        break;
                    }
                }


            }
            else
            {
                workerContiners[i].text_amount.text = "";
                workerContiners[i].text_button.text = "Max";
                workerContiners[i].upgradeSlider.value = 10;
            }
        }
    }

    public void ShowBarnData(int price, int barnCapacity)
    {
        if (price < 0)
        {
            barnUpgradeText.text = "Max";
            barnUpgradbutton.interactable = false;
        }
        else
        {
            barnUpgradeText.text = CoinSystem.ConvertCoinToString(price);
            barnUpgradbutton.interactable = true;
        }
        barnCapacityText.text = "Capacity: "+barnCapacity;
    }
    public void OnHireButtonPressed(int index)
    {
        hireButtonPressed?.Invoke(index);
    }
    public void OnClothButtonPressed(int index)
    {
        clothButtonPressed?.Invoke(index);
        
    }

    public void CloseButtonPressed()
    {
        
        closeButtonPressed?.Invoke();
    }

    public void OpenCloseBarnUI(bool isActive)
    {
        barnUIContiner.SetActive(isActive);
    }

    public void SetInteractStatus()
    {

    }

    

   
}





