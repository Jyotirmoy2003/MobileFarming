using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DressingRoomManager : MonoBehaviour
{
    public List<DressSetupUI> dressSetupsUI;
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

            for (int i = 0; i < dressSetupsUI.Count; i++)
            {
                dressSetupSave.dressesIspPurchessed.Add(false); // All not purchased by default
            }

            Save(); // Save new data
        }

        // Update local dress data from saved info
        for (int i = 0; i < dressSetupsUI.Count; i++)
        {
            if (i < dressSetupSave.dressesIspPurchessed.Count)
                dressSetupsUI[i].isPurched = dressSetupSave.dressesIspPurchessed[i];
        }
    }

    void Save()
    {
        for (int i = 0; i < dressSetupsUI.Count; i++)
        {
            if (i < dressSetupSave.dressesIspPurchessed.Count)
                dressSetupSave.dressesIspPurchessed[i] = dressSetupsUI[i].isPurched;
            else
                dressSetupSave.dressesIspPurchessed.Add(dressSetupsUI[i].isPurched);
        }

        SaveAndLoad.Save(dataPath,dressSetupSave);
    }

    void InitializeDresses()
    {
        foreach (var dress in dressSetupsUI)
        {
           
            dress.gemPriceText.text = dress.isPurched ? "Purchased" : dress.GemPrice.ToString();
            dress.gemIcongameObject.SetActive(!dress.isPurched);
            
        }
    }

    public void OnDressSelectd(int index)
    {
        if (dressSetupsUI[index].isPurched)
        {
            onDressSelected.Raise(this, dressSetupsUI[index].dressSetup);
        }
        else
        {
            if (CashManager.Instance.DebitGems(dressSetupsUI[index].GemPrice))
            {
                dressSetupsUI[index].isPurched = true;
                dressSetupsUI[index].gemIcongameObject.SetActive(false);
                Save();
                onDressSelected.Raise(this, dressSetupsUI[index].dressSetup);
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

[System.Serializable]
public class DressSetupUI
{
    public DressSetup dressSetup;
   [Tooltip("These are used only for the Dressing Room part not in game")]
   public bool isPurched = false;
   public int GemPrice = 40;
   public GameObject gemIcongameObject;

   public TMP_Text gemPriceText ;

}