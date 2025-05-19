using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using jy_util;
using TMPro;
using UnityEngine;

public class CashManager : MonoSingleton<CashManager>
{
     private int coins;
     private int gems;
     [SerializeField] TMP_Text coinsAmount_text;
     [SerializeField] TMP_Text gemsAmount_text;


     void Start()
     {
          LoadCurrency();
          coinsAmount_text.text = CoinSystem.ConvertCoinToString(coins);
          gemsAmount_text.text = CoinSystem.ConvertCoinToString(gems);
          //Invoke(nameof(Add50000Coin),1f);
     }
     #region Transaction
     public void CreditCoins(int amount)
     {
          coins+=amount;
          SaveCurrency();
          coinsAmount_text.text = CoinSystem.ConvertCoinToString(coins);
          AudioManager.instance.PlaySound("Coin");
     }

     public bool DebitCoin(int amount)
     {
          if(coins>=amount)
          {
               coins-=amount;
               SaveCurrency();
               coinsAmount_text.text = CoinSystem.ConvertCoinToString(coins);
               AudioManager.instance.PlaySound("Coin_Debit");
               return true;  
          }else{
               //UIManager.Instance.InsufficientGold();
               return false;
          }
     }

     public void CreditGems(int amount)
     {
          gems += amount;
          SaveCurrency();
          gemsAmount_text.text = CoinSystem.ConvertCoinToString(gems);
          AudioManager.instance.PlaySound("Gem");
     }

     public bool DebitGems(int amount)
     {
          if(gems>=amount)
          {
               // gems-=amount;
               // SaveCurrency();
               // gemsAmount_text.text = CoinSystem.ConvertCoinToString(gems);
               StartCoroutine(DebitGemType(amount));
               AudioManager.instance.PlaySound("Gem");
               return true;  
          }else{
               UIManager.Instance.InsufficientGem();
               return false;
          }
     }

     IEnumerator DebitGemType(int amountToDebit)
     {
          
          while(amountToDebit > 0)
          {
               yield return null;
               amountToDebit --;
               gems -=1;
               gemsAmount_text.text = CoinSystem.ConvertCoinToString(gems);
          }

          SaveCurrency();
     }
     
     
     
     #endregion

     private void SaveCurrency()
     {
          PlayerPrefs.SetInt("Coins",coins);
          PlayerPrefs.SetInt("Gems",gems);
     }

     private void LoadCurrency()
     {
          coins = PlayerPrefs.GetInt("Coins",0);
          gems = PlayerPrefs.GetInt("Gems",0);
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

     [NaughtyAttributes.Button]
     private void Add500Gems()
     {
          CreditCoins(500);
     }

     [NaughtyAttributes.Button]
     public void ClearGems()
     {
          DebitGems(gems);
     }
}
