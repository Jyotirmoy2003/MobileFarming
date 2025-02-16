using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using jy_util;
using TMPro;
using UnityEngine;

public class CashManager : MonoSingleton<CashManager>
{
     private int coins;
     [SerializeField] TMP_Text coinsAmount_text;


     void Start()
     {
          LoadCoins();
          coinsAmount_text.text = CoinSystem.ConvertCoinToString(coins);
          //Invoke(nameof(Add50000Coin),1f);
     }
     public void CreditCoins(int amount)
     {
          coins+=amount;
          SaveCoins();
          coinsAmount_text.text = CoinSystem.ConvertCoinToString(coins);
          AudioManager.instance.PlaySound("Coin");
     }

     public bool DebitCoin(int amount)
     {
          if(coins>=amount)
          {
               coins-=amount;
               SaveCoins();
               coinsAmount_text.text = CoinSystem.ConvertCoinToString(coins);
               AudioManager.instance.PlaySound("Coin_Debit");
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

     [NaughtyAttributes.Button]
     private void Add500Coin()
     {
          CreditCoins(500);
     }

      [NaughtyAttributes.Button]
     private void Add50000Coin()
     {
          CreditCoins(50000);
     }
     [NaughtyAttributes.Button]
     public void ClearCoin()
     {
          DebitCoin(coins);
     }
}
