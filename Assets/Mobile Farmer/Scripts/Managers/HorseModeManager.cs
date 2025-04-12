using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;
using System;

public class HorseModeManager : MonoSingleton<HorseModeManager>
{
    [Space]
    [SerializeField] GameEvent OnHorseModechanged;
    [SerializeField] bool isTesting = false;
    private string dataPath;
    private bool isHorseActive = false;
    private bool ishorsePurchesed = false;
    private AllSave allSave = new AllSave();

    void Start()
    {
        dataPath = Application.persistentDataPath + _GameAssets.AllSaveFileName;
        #if UNITY_EDITOR
        dataPath = Application.dataPath + _GameAssets.AllSaveFileName;
        #endif
        if(!isTesting)LoadData();
        CheckHorseAvailability();

        UIManager.Instance.ShowHorseButton(ishorsePurchesed);
    }

    public void OnHorsePurchased()
    {
        ishorsePurchesed = true;
        allSave.ishorsePurchesed = true;
        allSave.horsePurchesTime = DateTime.UtcNow.ToString("o"); // Save in ISO 8601 format
        SaveAndLoad.Save(dataPath,allSave);
    }

    [NaughtyAttributes.Button]
    public void OnHorseButtonpressed()
    {
        CheckHorseAvailability(); // ensure up-to-date status
        if (ishorsePurchesed)
        {
            OnHorseModechanged.Raise(this, isHorseActive = !isHorseActive);
        }
        else
        {
            Debug.Log("Horse time expired or not purchased.");
            // Show purchase UI or prompt here
        }
    }

    public bool IshorsePurchesed()
    {
        CheckHorseAvailability();
        return ishorsePurchesed;
    }

    void CheckHorseAvailability()
    {

        if (!string.IsNullOrEmpty(allSave.horsePurchesTime))
        {
            DateTime purchaseTime = DateTime.Parse(allSave.horsePurchesTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
            if ((DateTime.UtcNow - purchaseTime).TotalHours >= 1)
            {
                ishorsePurchesed = false;
                allSave.ishorsePurchesed = false;
                SaveAndLoad.Save(dataPath,allSave);
            }
            else
            {
                ishorsePurchesed = true;
            }
        }
        else
        {
            ishorsePurchesed = false;
        }
    }

    void LoadData()
    {
        AllSave loaded = SaveAndLoad.Load<AllSave>(dataPath);
        if (loaded != null)
        {
            allSave = loaded;
        }else
        {
            loaded = new AllSave();
        }
    }
}
