using System;
using System.Collections.Generic;
using jy_util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyLoginManager : MonoBehaviour
{
    private const string LAST_LOGIN_DATE = "LastLoginDate";
    private const string CURRENT_DAY = "CurrentLoginDay";
    private int currentDay = 0;
    [SerializeField] GameObject DailyRewardPanel;
    [SerializeField] List<DailyRewards> dailyLoginPanels = new List<DailyRewards>();
    
    [SerializeField] Color deactivateColor;

    private void Start()
    {
        Invoke(nameof(CheckDailyLogin),4f);
    }

    public void CheckDailyLogin()
    {
        string lastLogin = PlayerPrefs.GetString(LAST_LOGIN_DATE, "");
        //string lastLogin = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        if (lastLogin != today) // If user hasn't logged in today
        {
            _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
            currentDay = PlayerPrefs.GetInt(CURRENT_DAY, -1) + 1;
            if (currentDay >= 7) currentDay = 1; // Reset after 7 days

            PlayerPrefs.SetInt(CURRENT_DAY, currentDay);
            PlayerPrefs.SetString(LAST_LOGIN_DATE, today);
            PlayerPrefs.Save();

            GiveReward(currentDay);
        }
       
    }

    void DeactivateDay(int index)
    {
        dailyLoginPanels[index].panel_img.color = deactivateColor;
    }
    void ActivateDay(int index)
    {
        dailyLoginPanels[index].panel_img.color = dailyLoginPanels[index].panelColor;
    }

    private void GiveReward(int day)
    {
        Debug.Log($"Day {day} reward claimed!");

        //show correct Icon
        for(int i=0 ; i<dailyLoginPanels.Count;i++){
            dailyLoginPanels[i].Icon.sprite = _GameAssets.Instance.GetItemIcon(dailyLoginPanels[i].e_Inventory_Item_Type);
            dailyLoginPanels[i].amountText.text = "x"+dailyLoginPanels[i].amount.ToString();
        
        }




        DailyRewardPanel.SetActive(true);

        UIManager.Instance.OnUniversalCloseButtonPressed += ClosePanelPressed;
        UIManager.Instance.SetCloseButton(true);
        // TODO: Implement your reward system (gold, items, etc.)
        for(int i=0 ;i<day ; i++)
        {
            DeactivateDay(i);
        }
        for(int i=day ;i<7 ; i++)
        {
            ActivateDay(i);
        }

        if(dailyLoginPanels[day].e_Inventory_Item_Type != E_Inventory_Item_Type.None)
        {
            UIManager.Instance.ItemCreadited(dailyLoginPanels[day].e_Inventory_Item_Type,dailyLoginPanels[day].amount);
            InventoryManager.Instance.AddItemToInventory(dailyLoginPanels[day].e_Inventory_Item_Type,dailyLoginPanels[day].amount);
        }

        Invoke(nameof(DeactivateToday),2f);
    }

    void DeactivateToday()
    {
        DeactivateDay(currentDay);
    }

    void ClosePanelPressed()
    {
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,false);
        UIManager.Instance.OnUniversalCloseButtonPressed -= ClosePanelPressed;
        UIManager.Instance.SetCloseButton(false);
        DailyRewardPanel.SetActive(false);
    }

    

    public int GetCurrentDay()
    {
        return PlayerPrefs.GetInt(CURRENT_DAY, 1);
    }
    



}

[System.Serializable]
public struct DailyRewards
{
    public Image panel_img;
    public Image Icon;
    public TMP_Text amountText;
    public Color panelColor;
    public E_Inventory_Item_Type e_Inventory_Item_Type;
    public int amount;

}
