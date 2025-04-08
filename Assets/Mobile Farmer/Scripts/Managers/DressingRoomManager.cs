using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DressingRoomManager : MonoBehaviour
{
    public List<DressSetup> dressSetups;
    public GameEvent onDressSelected;

    private DressSetupSave dressSetupSave;
    private string dataPath;
    private string saveFileName = "/dresses.txt";

    void Start()
    {
        Load();
        Invoke(nameof(InitializeDresses),0.2f);
    }

    void Load()
    {
        dataPath = Application.persistentDataPath + saveFileName;

        #if UNITY_EDITOR
                dataPath = Application.dataPath + saveFileName;
        #endif

        dressSetupSave = SaveAndLoad.Load<DressSetupSave>(dataPath);

        if (dressSetupSave == null )
        {
            // Initialize new save data
            dressSetupSave = new DressSetupSave();
            dressSetupSave.dressesIspPurchessed = new List<bool>();

            for (int i = 0; i < dressSetups.Count; i++)
            {
                dressSetupSave.dressesIspPurchessed.Add(false); // All not purchased by default
            }

            Save(); // Save new data
        }

        // Update local dress data from saved info
        for (int i = 0; i < dressSetups.Count; i++)
        {
            if (i < dressSetupSave.dressesIspPurchessed.Count)
                dressSetups[i].isPurched = dressSetupSave.dressesIspPurchessed[i];
        }
    }

    void Save()
    {
        for (int i = 0; i < dressSetups.Count; i++)
        {
            if (i < dressSetupSave.dressesIspPurchessed.Count)
                dressSetupSave.dressesIspPurchessed[i] = dressSetups[i].isPurched;
            else
                dressSetupSave.dressesIspPurchessed.Add(dressSetups[i].isPurched);
        }

        SaveAndLoad.Save(dataPath,dressSetupSave);
    }

    void InitializeDresses()
    {
        foreach (var dress in dressSetups)
        {
           
            dress.gemPriceText.text = dress.isPurched ? "Purchased" : dress.GemPrice.ToString();
            
        }
    }

    public void OnDressSelectd(int index)
    {
        if (dressSetups[index].isPurched)
        {
            onDressSelected.Raise(this, dressSetups[index]);
        }
        else
        {
            if (CashManager.Instance.DebitGems(dressSetups[index].GemPrice))
            {
                dressSetups[index].isPurched = true;
                Save();
                onDressSelected.Raise(this, dressSetups[index]);
                InitializeDresses();
            }
            else
            {
                Debug.Log("<color=green>Insufficient gems</color>");
            }
        }
    }
}

[System.Serializable]
public class DressSetupSave
{
    public List<bool> dressesIspPurchessed = new List<bool>();
}
