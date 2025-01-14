using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class CashManager : MonoSingleton<CashManager>
{
    private int coins;
    [SerializeField] TMP_Text coinsAmount_text;


    void Start()
    {
        LoadCoins();
        coinsAmount_text.text = coins.ToString();;
    }
   public void CreditCoins(int amount)
   {
        coins+=amount;
        SaveCoins();

        Debug.Log("We now have :"+coins +" coins");
        coinsAmount_text.text = coins.ToString();
   }

   public bool DebitCoin(int amount)
   {
        if(coins>=amount)
        {
            coins-=amount;
            SaveCoins();
            coinsAmount_text.text = coins.ToString();;
            return true;  
        }else{
            return false;
        }
   }



   private void SaveCoins()
   {
        PlayerPrefs.SetInt("Coins",coins);
   }

   private void LoadCoins()
   {
        coins = PlayerPrefs.GetInt("Coins",0);
   }
}
